﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FontParser.Exceptions;

namespace FontParser.Tables.CFF.CFF
{
    internal class Cff1Parser
    {
        //from: Adobe's The Compact Font Format Specification, version1.0, Dec 2003

        //Table 2 CFF Data Types
        //Name       Range          Description
        //Card8      0 – 255   	    1-byte unsigned number
        //Card16     0 – 65535 	    2-byte unsigned number
        //Offset     varies 	  	1, 2, 3, or 4 byte offset(specified by  OffSize field)
        //OffSize	 1–4			1-byte unsigned number specifies the
        //                          size of an Offset field or fields
        //SID		0 – 64999       2-byte string identifier
        //-----------------

        //Table 1 CFF Data Layout
        //Entry                     Comments
        //Header      		        –
        //Name INDEX  		        –
        //Top DICT INDEX 		    –
        //String INDEX		        –
        //Global Subr INDEX	        –
        //Encodings			        –
        //Charsets			        –
        //FDSelect                  CIDFonts only
        //CharStrings INDEX         per-font
        //Font DICT INDEX           per-font, CIDFonts only
        //Private DICT              per-font
        //Local Subr INDEX          per-font or per-Private DICT for CIDFonts
        //Copyright and Trademark	-
        // Notices
        //-----------------

        //from Apache's PDF box/FontBox
        //@author Villu Ruusmann

        private BinaryReader _reader;

        private Cff1FontSet _cff1FontSet;
        private Cff1Font _currentCff1Font;

        private List<CffDataDicEntry> _topDic;

        private long _cffStartAt;

        private int _charStringsOffset;
        private int _charsetOffset;
        private int _encodingOffset;

        public void ParseAfterHeader(long cffStartAt, BinaryReader reader)
        {
            _cffStartAt = cffStartAt;
            _cff1FontSet = new Cff1FontSet();
            _cidFontInfo = new CIDFontInfo();
            _reader = reader;

            //
            ReadNameIndex();
            ReadTopDICTIndex();
            ReadStringIndex();
            ResolveTopDictInfo();
            ReadGlobalSubrIndex();

            //----------------------
            ReadFDSelect();
            ReadFDArray();

            ReadPrivateDict();

            ReadCharStringsIndex();
            ReadCharsets();
            ReadEncodings();

            //...
        }

        public Cff1FontSet ResultCff1FontSet => _cff1FontSet;

        //
        private void ReadNameIndex()
        {
            //7. Name INDEX
            //This contains the PostScript language names(FontName or
            //CIDFontName) of all the fonts in the FontSet stored in an INDEX
            //structure.The font names are sorted, thereby permitting a
            //binary search to be performed when locating a specific font
            //within a FontSet. The sort order is based on character codes
            //treated as 8 - bit unsigned integers. A given font name precedes
            //  another font name having the first name as its prefix.There
            //  must be at least one entry in this INDEX, i.e.the FontSet must
            // contain at least one font.

            //For compatibility with client software, such as PostScript
            //interpreters and Acrobat®, font names should be no longer
            //than 127 characters and should not contain any of the following
            //ASCII characters: [, ], (, ), {, }, <, >, /, %, null(NUL), space, tab,
            //carriage return, line feed, form feed.It is recommended that
            //font names be restricted to the printable ASCII subset, codes 33
            //through 126.Adobe Type Manager® (ATM®) software imposes
            //a further restriction on the font name length of 63 characters.

            //Note 3
            //For compatibility with earlier PostScript
            //interpreters, see Technical Note
            //#5088, “Font Naming Issues.”

            //A font may be deleted from a FontSet without removing its data
            //by setting the first byte of its name in the Name INDEX to 0
            //(NUL).This kind of deletion offers a simple way to handle font
            //upgrades without rebuilding entire fontsets.Binary search
            //software must detect deletions and restart the search at the
            //previous or next name in the INDEX to ensure that all
            //appropriate names are matched.

            CffIndexOffset[] nameIndexElems = ReadIndexDataOffsets();
            if (nameIndexElems == null) return;
            //

            int count = nameIndexElems.Length;
            string[] fontNames = new string[count];
            for (var i = 0; i < count; ++i)
            {
                //read each FontName or CIDFontName
                CffIndexOffset indexElem = nameIndexElems[i];
                //TODO: review here again,
                //check if we need to set _reader.BaseStream.Position or not
                fontNames[i] = Encoding.UTF8.GetString(_reader.ReadBytes(indexElem.len), 0, indexElem.len);
            }

            //
            _cff1FontSet._fontNames = fontNames;

            //TODO: review here
            //in this version
            //count ==1
            if (count != 1)
            {
                throw new OpenFontNotSupportedException();
            }
            _currentCff1Font = new Cff1Font
            {
                FontName = fontNames[0]
            };
            _cff1FontSet._fonts.Add(_currentCff1Font);
        }

        private void ReadTopDICTIndex()
        {
            //8. Top DICT INDEX
            //This contains the top - level DICTs of all the fonts in the FontSet
            //stored in an INDEX structure.Objects contained within this
            //INDEX correspond to those in the Name INDEX in both order
            //and number. Each object is a DICT structure that corresponds to
            //the top-level dictionary of a PostScript font.
            //A font is identified by an entry in the Name INDEX and its data
            //is accessed via the corresponding Top DICT
            CffIndexOffset[] offsets = ReadIndexDataOffsets();

            //9. Top DICT Data
            //The names of the Top DICT operators shown in
            //Table 9 are, where possible, the same as the corresponding Type 1 dict key.
            //Operators that have no corresponding Type1 dict key are noted
            //in the table below along with a default value, if any. (Several
            //operators have been derived from FontInfo dict keys but have
            //been grouped together with the Top DICT operators for
            //simplicity.The keys from the FontInfo dict are indicated in the
            //Default, notes  column of Table 9)
            int count = offsets.Length;
            if (count > 1)
            {
                //temp...
                //TODO: review here again
                throw new OpenFontNotSupportedException();
            }
            for (var i = 0; i < count; ++i)
            {
                //read DICT data

                List<CffDataDicEntry> dicData = ReadDICTData(offsets[i].len);
                _topDic = dicData;
            }
        }

        private string[] _uniqueStringTable;

        private struct CIDFontInfo
        {
            public string ROS_Register;
            public string ROS_Ordering;
            public string ROS_Supplement;

            public double CIDFontVersion;
            public int CIDFountCount;
            public int FDSelect;
            public int FDArray;

            public int fdSelectFormat;
            public FDRange3[] fdRanges;
        }

        private CIDFontInfo _cidFontInfo;

        private void ReadStringIndex()
        {
            //10 String INDEX

            //All the strings, with the exception of the FontName and
            //CIDFontName strings which appear in the Name INDEX, used by
            //different fonts within the FontSet are collected together into an
            //INDEX structure and are referenced by a 2 - byte unsigned
            //number called a string identifier or SID.

            //Only unique strings are stored in the table
            //thereby removing duplication across fonts.

            //Further space saving is obtained by allocating commonly
            //occurring strings to predefined SIDs.
            //These strings, known as the standard strings,
            //describe all the names used in the ISOAdobe and
            //Expert character sets along with a few other strings
            //common to Type 1 fonts.

            //A complete list of standard strings is given in Appendix A

            //The client program will contain an array of standard strings with
            //nStdStrings elements.
            //Thus, the standard strings take SIDs in the
            //range 0 to(nStdStrings –1).

            //The first string in the String INDEX
            //corresponds to the SID whose value is equal to nStdStrings, the
            //first non - standard string, and so on.

            //When the client needs to
            //determine the string that corresponds to a particular SID it
            //performs the following: test if SID is in standard range then
            //fetch from internal table,
            //otherwise, fetch string from the String
            //INDEX using a value of(SID – nStdStrings) as the index.

            CffIndexOffset[] offsets = ReadIndexDataOffsets();
            if (offsets == null) return;
            //

            _uniqueStringTable = new string[offsets.Length];

            var buff = new byte[512];//reusable

            for (var i = 0; i < offsets.Length; ++i)
            {
                int len = offsets[i].len;
                //TODO: review here again,
                //check if we need to set _reader.BaseStream.Position or not
                //TODO: Is Charsets.ISO_8859_1 Encoding supported in .netcore
                if (len < buff.Length)
                {
                    int actualRead = _reader.Read(buff, 0, len);
#if DEBUG
                    if (actualRead != len)
                    {
                        throw new OpenFontNotSupportedException();
                    }
#endif
                    _uniqueStringTable[i] = Encoding.UTF8.GetString(buff, 0, len);
                }
                else
                {
                    _uniqueStringTable[i] = Encoding.UTF8.GetString(_reader.ReadBytes(len), 0, len);
                }
            }

            _cff1FontSet._uniqueStringTable = _uniqueStringTable;
        }

        private string GetSid(int sid)
        {
            if (sid <= Cff1FontSet.N_STD_STRINGS)
            {
                //use standard name
                //TODO: review here
                return Cff1FontSet.s_StdStrings[sid];
            }
            else
            {
                if (sid - Cff1FontSet.N_STD_STRINGS - 1 < _uniqueStringTable.Length)
                {
                    return _uniqueStringTable[sid - Cff1FontSet.N_STD_STRINGS - 1];
                }
                else
                {
                    //skip this,
                    //eg. found in CID font,
                    //we should provide this info later

                    return null;
                }
            }
        }

        private void ResolveTopDictInfo()
        {
            //translate top-dic***
            foreach (CffDataDicEntry entry in _topDic)
            {
                switch (entry._operator.Name)
                {
                    default:
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine("topdic:" + entry._operator.Name);
#endif
                        }
                        break;

                    case "XUID": break;//nothing
                    case "version":
                        _currentCff1Font.Version = GetSid((int)entry.operands[0]._realNumValue);
                        break;

                    case "Notice":
                        _currentCff1Font.Notice = GetSid((int)entry.operands[0]._realNumValue);
                        break;

                    case "Copyright":
                        _currentCff1Font.CopyRight = GetSid((int)entry.operands[0]._realNumValue);
                        break;

                    case "FullName":
                        _currentCff1Font.FullName = GetSid((int)entry.operands[0]._realNumValue);
                        break;

                    case "FamilyName":
                        _currentCff1Font.FamilyName = GetSid((int)entry.operands[0]._realNumValue);
                        break;

                    case "Weight":
                        _currentCff1Font.Weight = GetSid((int)entry.operands[0]._realNumValue);
                        break;

                    case "UnderlinePosition":
                        _currentCff1Font.UnderlinePosition = entry.operands[0]._realNumValue;
                        break;

                    case "UnderlineThickness":
                        _currentCff1Font.UnderlineThickness = entry.operands[0]._realNumValue;
                        break;

                    case "FontBBox":
                        _currentCff1Font.FontBBox = new double[] {
                            entry.operands[0]._realNumValue,
                            entry.operands[1]._realNumValue,
                            entry.operands[2]._realNumValue,
                            entry.operands[3]._realNumValue};
                        break;

                    case "CharStrings":
                        _charStringsOffset = (int)entry.operands[0]._realNumValue;
                        break;

                    case "charset":
                        _charsetOffset = (int)entry.operands[0]._realNumValue;
                        break;

                    case "Encoding":
                        _encodingOffset = (int)entry.operands[0]._realNumValue;
                        break;

                    case "Private":
                        //private DICT size and offset
                        _privateDICTLen = (int)entry.operands[0]._realNumValue;
                        _privateDICTOffset = (int)entry.operands[1]._realNumValue;
                        break;

                    case "ROS":
                        //http://wwwimages.adobe.com/www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5176.CFF.pdf
                        //A CFF CIDFont has the CIDFontName in the Name INDEX and a corresponding Top DICT.
                        //The Top DICT begins with ROS operator which specifies the Registry-Ordering - Supplement for the font.
                        //This will indicate to a CFF parser that special CID processing should be applied to this font. Specifically:

                        //ROS operator combines the Registry, Ordering, and Supplement keys together.

                        //see Adobe Cmap resource , https://github.com/adobe-type-tools/cmap-resources

                        _cidFontInfo.ROS_Register = GetSid((int)entry.operands[0]._realNumValue);
                        _cidFontInfo.ROS_Ordering = GetSid((int)entry.operands[1]._realNumValue);
                        _cidFontInfo.ROS_Supplement = GetSid((int)entry.operands[2]._realNumValue);

                        break;

                    case "CIDFontVersion":
                        _cidFontInfo.CIDFontVersion = entry.operands[0]._realNumValue;
                        break;

                    case "CIDCount":
                        _cidFontInfo.CIDFountCount = (int)entry.operands[0]._realNumValue;
                        break;

                    case "FDSelect":
                        _cidFontInfo.FDSelect = (int)entry.operands[0]._realNumValue;
                        break;

                    case "FDArray":
                        _cidFontInfo.FDArray = (int)entry.operands[0]._realNumValue;
                        break;
                }
            }
        }

        private void ReadGlobalSubrIndex()
        {
            //16. Local / Global Subrs INDEXes
            //Both Type 1 and Type 2 charstrings support the notion of
            //subroutines or subrs.

            //A subr is typically a sequence of charstring
            //bytes representing a sub - program that occurs in more than one
            //  place in a font’s charstring data.

            //This subr may be stored once
            //but referenced many times from within one or more charstrings
            //by the use of the call subr  operator whose operand is the
            //number of the subr to be called.

            //The subrs are local to a  particular font and
            //cannot be shared between fonts.

            //Type 2 charstrings also permit global subrs which function in the same
            //way but are called by the call gsubr operator and may be shared
            //across fonts.

            //Local subrs are stored in an INDEX structure which is located via
            //the offset operand of the Subrs  operator in the Private DICT.
            //A font without local subrs has no Subrs operator in the Private DICT.

            //Global subrs are stored in an INDEX structure which follows the
            //String INDEX. A FontSet without any global subrs is represented
            //by an empty Global Subrs INDEX.
            _currentCff1Font._globalSubrRawBufferList = ReadSubrBuffer();
        }

        private void ReadLocalSubrs()
        {
            _currentCff1Font._localSubrRawBufferList = ReadSubrBuffer();
        }

        private void ReadEncodings()
        {
            //Encoding data is located via the offset operand to the
            //Encoding operator in the Top DICT.

            //Only one Encoding operator can be
            //specified per font except for CIDFonts which specify no
            //encoding.

            //A glyph’s encoding is specified by a 1 - byte code that
            //permits values in the range 0 - 255.

            //Each encoding is described by a format-type identifier byte
            //followed by format-specific data.Two formats are currently
            //defined as specified in Tables 11(Format 0) and 12(Format 1).
            byte format = _reader.ReadByte();
            switch (format)
            {
                default:
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("cff_parser_read_encodings:" + format);
#endif
                    break;

                case 0:
                    ReadFormat0Encoding();
                    break;

                case 1:
                    ReadFormat1Encoding();
                    break;
            }
            //TODO: ...
        }

        private void ReadCharsets()
        {
            //Charset data is located via the offset operand to the
            //charset operator in the Top DICT.

            //Each charset is described by a format-
            //type identifier byte followed by format-specific data.
            //Three formats are currently defined as shown in Tables
            //17, 18, and 20.

            _reader.BaseStream.Position = _cffStartAt + _charsetOffset;
            //TODO: ...
            byte format = _reader.ReadByte();
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 0:
                    ReadCharsetsFormat0();
                    break;

                case 1:
                    ReadCharsetsFormat1();
                    break;

                case 2:
                    ReadCharsetsFormat2();
                    break;
            }
        }

        private void ReadCharsetsFormat0()
        {
            //Table 17: Format 0
            //Type	    Name		        Description
            //Card8     format    		    =0
            //SID       glyph[nGlyphs-1] 	Glyph name array

            //Each element of the glyph array represents the name of the
            //corresponding glyph. This format should be used when the SIDs
            //are in a fairly random order. The number of glyphs (nGlyphs) is
            //the value of the count field in the
            //CharStrings INDEX. (There is
            //one less element in the glyph name array than nGlyphs because
            //the .notdef glyph name is omitted.)

            Glyph[] cff1Glyphs = _currentCff1Font._glyphs;
            int nGlyphs = cff1Glyphs.Length;
            for (var i = 1; i < nGlyphs; ++i)
            {
                Cff1GlyphData d = cff1Glyphs[i]._cff1GlyphData;
                d.Name = GetSid(d.SIDName = _reader.ReadUInt16());
            }
        }

        private void ReadCharsetsFormat1()
        {
            //Table 18 Format 1
            //Type		Name	            Description
            //Card8		format		        =1
            //struct	Range1[<varies>]	Range1 array (see Table  19)

            //Table 19 Range1 Format (Charset)
            //Type      Name          Description
            //SID       first         First glyph in range
            //Card8     nLeft         Glyphs left in range(excluding first)

            //Each Range1 describes a group of sequential SIDs. The number
            //of ranges is not explicitly specified in the font. Instead, software
            //utilizing this data simply processes ranges until all glyphs in the
            //font are covered. This format is particularly suited to charsets
            //that are well ordered

            // throw new OpenFontNotSupportedException();
            Glyph[] cff1Glyphs = _currentCff1Font._glyphs;
            int nGlyphs = cff1Glyphs.Length;
            for (var i = 1; i < nGlyphs;)
            {
                int sid = _reader.ReadUInt16();// First glyph in range
                int count = _reader.ReadByte() + 1;//since it not include first elem
                do
                {
                    Cff1GlyphData d = cff1Glyphs[i]._cff1GlyphData;
                    d.Name = GetSid(d.SIDName = (ushort)sid);

                    count--;
                    i++;
                    sid++;
                } while (count > 0);
            }
        }

        private void ReadCharsetsFormat2()
        {
            //note:eg, Adobe's source-code-pro font

            //Table 20 Format 2
            //Type          Name              Description
            //Card8         format            2
            //struct        Range2[<varies>]  Range2 array (see Table 21)
            //
            //-----------------------------------------------
            //Table 21 Range2 Format
            //Type          Name             Description
            //SID           first            First glyph in range
            //Card16        nLeft           Glyphs left in range (excluding first)
            //-----------------------------------------------

            //Format 2 differs from format 1 only in the size of the nLeft field in each range.
            //This format is most suitable for fonts with a large well - ordered charset — for example, for Asian CIDFonts.

            Glyph[] cff1Glyphs = _currentCff1Font._glyphs;
            int nGlyphs = cff1Glyphs.Length;
            for (var i = 1; i < nGlyphs;)
            {
                int sid = _reader.ReadUInt16();// First glyph in range
                int count = _reader.ReadUInt16() + 1;//since it not include first elem
                do
                {
                    Cff1GlyphData d = cff1Glyphs[i]._cff1GlyphData;
                    d.Name = GetSid(d.SIDName = (ushort)sid);

                    count--;
                    i++;
                    sid++;
                } while (count > 0);
            }
        }

        private void ReadFDSelect()
        {
            //http://wwwimages.adobe.com/www.adobe.com/content/dam/acom/en/devnet/font/pdfs/5176.CFF.pdf
            //19. FDSelect

            //The FDSelect associates an FD(Font DICT) with a glyph by
            //specifying an FD index for that glyph. The FD index is used to
            //access one of the Font DICTs stored in the Font DICT INDEX.

            //FDSelect data is located via the offset operand to the
            //FDSelect operator in the Top DICT.FDSelect data specifies a format - type
            //identifier byte followed by format-specific data.Two formats
            //are currently defined, as shown in Tables  27 and 28.
            //TODO: ...

            //FDSelect   12 37  number      –, FDSelect offset
            if (_cidFontInfo.FDSelect == 0) { return; }

            //
            //Table 27 Format 0
            //------------
            //Type    Name              Description
            //------------
            //Card8   format            =0
            //Card8   fd[nGlyphs]       FD selector array

            //Each element of the fd array(fds) represents the FD index of the corresponding glyph.
            //This format should be used when the FD indexes are in a fairly random order.
            //The number of glyphs(nGlyphs) is the value of the count field in the CharStrings INDEX.
            //(This format is identical to charset format 0 except that the.notdef glyph is included in this case.)

            //Table 28 Format 3
            //------------
            //Type    Name              Description
            //------------
            //Card8   format            =3
            //Card16  nRanges           Number of ranges
            //struct  Range3[nRanges]   Range3 array (see Table 29)
            //Card16  sentinel          Sentinel GID (see below)
            //------------

            //Table 29 Range3
            //------------
            //Type    Name              Description
            //------------
            //Card16  first             First glyph index in range
            //Card8   fd                FD index for all glyphs in range

            //Each Range3 describes a group of sequential GIDs that have the same FD index.
            //Each range includes GIDs from the ‘first’ GID up to, but not including,
            //the ‘first’ GID of the next range element.
            //Thus, elements of the Range3 array are ordered by increasing ‘first’ GIDs.
            //The first range must have a ‘first’ GID of 0.
            //A sentinel GID follows the last range element and serves to delimit the last range in the array.
            //(The sentinel GID is set equal to the number of glyphs in the font.
            //That is, its value is 1 greater than the last GID in the font.)
            //This format is particularly suited to FD indexes that are well ordered(the usual case).

            _reader.BaseStream.Position = _cffStartAt + _cidFontInfo.FDSelect;
            byte format = _reader.ReadByte();

            switch (format)
            {
                default:
                    throw new OpenFontNotSupportedException();
                case 3:
                    {
                        ushort nRanges = _reader.ReadUInt16();
                        var ranges = new FDRange3[nRanges + 1];

                        _cidFontInfo.fdSelectFormat = 3;
                        _cidFontInfo.fdRanges = ranges;
                        for (var i = 0; i < nRanges; ++i)
                        {
                            ranges[i] = new FDRange3(_reader.ReadUInt16(), _reader.ReadByte());
                        }

                        //end with //sentinel
                        ranges[nRanges] = new FDRange3(_reader.ReadUInt16(), 0);//sentinel
#if DEBUG

#endif
                    }
                    break;
            }
        }

        private readonly struct FDRange3
        {
            public readonly ushort first;
            public readonly byte fd;

            public FDRange3(ushort first, byte fd)
            {
                this.first = first;
                this.fd = fd;
            }

#if DEBUG

            public override string ToString()
            {
                return "first:" + first + ",fd:" + fd;
            }

#endif
        }

        private void ReadFDArray()
        {
            if (_cidFontInfo.FDArray == 0) { return; }
            _reader.BaseStream.Position = _cffStartAt + _cidFontInfo.FDArray;

            CffIndexOffset[] offsets = ReadIndexDataOffsets();

            List<FontDict> fontDicts = new List<FontDict>();
            _currentCff1Font._cidFontDict = fontDicts;

            for (var i = 0; i < offsets.Length; ++i)
            {
                //read DICT data
                List<CffDataDicEntry> dic = ReadDICTData(offsets[i].len);
                //translate

                var offset = 0;
                var size = 0;
                var name = 0;

                foreach (CffDataDicEntry entry in dic)
                {
                    switch (entry._operator.Name)
                    {
                        default: throw new OpenFontNotSupportedException();
                        case "FontName":
                            name = (int)entry.operands[0]._realNumValue;
                            break;

                        case "Private": //private dic
                            size = (int)entry.operands[0]._realNumValue;
                            offset = (int)entry.operands[1]._realNumValue;
                            break;
                    }
                }

                var fontdict = new FontDict(size, offset)
                {
                    FontName = name
                };
                fontDicts.Add(fontdict);
            }
            //-----------------

            foreach (FontDict fdict in fontDicts)
            {
                _reader.BaseStream.Position = _cffStartAt + fdict.PrivateDicOffset;

                List<CffDataDicEntry> dicData = ReadDICTData(fdict.PrivateDicSize);

                if (dicData.Count > 0)
                {
                    //interpret the values of private dict
                    foreach (CffDataDicEntry dicEntry in dicData)
                    {
                        switch (dicEntry._operator.Name)
                        {
                            case "Subrs":
                                {
                                    var localSubrsOffset = (int)dicEntry.operands[0]._realNumValue;
                                    _reader.BaseStream.Position = _cffStartAt + fdict.PrivateDicOffset + localSubrsOffset;
                                    fdict.LocalSubr = ReadSubrBuffer();
                                }
                                break;

                            case "defaultWidthX":

                                break;

                            case "nominalWidthX":

                                break;

                            default:
                                {
#if DEBUG
                                    System.Diagnostics.Debug.WriteLine("cff_pri_dic:" + dicEntry._operator.Name);
#endif
                                }
                                break;
                        }
                    }
                }
            }
        }

        private struct FDRangeProvider
        {
            //helper class

            private FDRange3[] _ranges;
            private ushort _currentGlyphIndex;
            private ushort _endGlyphIndexLim;
            private byte _selectedFdArray;
            private FDRange3 _currentRange;
            private int _currentSelectedRangeIndex;

            public FDRangeProvider(FDRange3[] ranges)
            {
                _ranges = ranges;
                _currentGlyphIndex = 0;
                _currentSelectedRangeIndex = 0;

                if (ranges != null)
                {
                    _currentRange = ranges[0];
                    _endGlyphIndexLim = ranges[1].first;
                }
                else
                {
                    //empty
                    _currentRange = new FDRange3();
                    _endGlyphIndexLim = 0;
                }
                _selectedFdArray = 0;
            }

            public byte SelectedFDArray => _selectedFdArray;

            public void SetCurrentGlyphIndex(ushort index)
            {
                //find proper range for selected index
                if (index >= _currentRange.first && index < _endGlyphIndexLim)
                {
                    //ok, in current range
                    _selectedFdArray = _currentRange.fd;
                }
                else
                {
                    //move to next range
                    _currentSelectedRangeIndex++;
                    _currentRange = _ranges[_currentSelectedRangeIndex];

                    _endGlyphIndexLim = _ranges[_currentSelectedRangeIndex + 1].first;
                    if (index >= _currentRange.first && index < _endGlyphIndexLim)
                    {
                        _selectedFdArray = _currentRange.fd;
                    }
                    else
                    {
                        throw new OpenFontNotSupportedException();
                    }
                }
                _currentGlyphIndex = index;
            }
        }

        private void ReadCharStringsIndex()
        {
            //14. CharStrings INDEX

            //This contains the charstrings of all the glyphs in a font stored in
            //an INDEX structure.

            //Charstring objects contained within this
            //INDEX are accessed by GID.

            //The first charstring(GID 0) must be
            //the.notdef glyph.

            //The number of glyphs available in a font may
            //be determined from the count field in the INDEX.

            //

            //The format of the charstring data, and therefore the method of
            //interpretation, is specified by the
            //CharstringType  operator in the Top DICT.

            //The CharstringType operator has a default value
            //of 2 indicating the Type 2 charstring format which was designed
            //in conjunction with CFF.

            //Type 1 charstrings are documented in
            //the “Adobe Type 1 Font Format” published by Addison - Wesley.

            //Type 2 charstrings are described in Adobe Technical Note #5177:
            //“Type 2 Charstring Format.” Other charstring types may also be
            //supported by this method.

            _reader.BaseStream.Position = _cffStartAt + _charStringsOffset;
            CffIndexOffset[] offsets = ReadIndexDataOffsets();

#if DEBUG
            if (offsets.Length > ushort.MaxValue) { throw new OpenFontNotSupportedException(); }
#endif
            int glyphCount = offsets.Length;
            //assume Type2
            //TODO: review here

            Glyph[] glyphs = new Glyph[glyphCount];
            _currentCff1Font._glyphs = glyphs;
            var type2Parser = new Type2CharStringParser();
            type2Parser.SetCurrentCff1Font(_currentCff1Font);

#if DEBUG
            double total = 0;
#endif

            //cid font or not

            var fdRangeProvider = new FDRangeProvider(_cidFontInfo.fdRanges);
            bool isCidFont = _cidFontInfo.fdRanges != null;

            for (var i = 0; i < glyphCount; ++i)
            {
                CffIndexOffset offset = offsets[i];
                byte[] buffer = _reader.ReadBytes(offset.len);
#if DEBUG
                //check
                byte lastByte = buffer[offset.len - 1];
                if (lastByte != (byte)Type2Operator1.endchar &&
                    lastByte != (byte)Type2Operator1.callgsubr &&
                    lastByte != (byte)Type2Operator1.callsubr)
                {
                    //5177.Type2
                    //Note 6 The charstring itself may end with a call(g)subr; the subroutine must
                    //then end with an endchar operator
                    //endchar
                    throw new Exception("invalid end byte?");
                }
#endif
                //now we can parse the raw glyph instructions

                var glyphData = new Cff1GlyphData();
#if DEBUG
                type2Parser.dbugCurrentGlyphIndex = (ushort)i;
#endif

                if (isCidFont)
                {
                    //select  proper local private dict
                    fdRangeProvider.SetCurrentGlyphIndex((ushort)i);
                    type2Parser.SetCidFontDict(_currentCff1Font._cidFontDict[fdRangeProvider.SelectedFDArray]);
                }

                Type2GlyphInstructionList instList = type2Parser.ParseType2CharString(buffer);
                if (instList != null)
                {
                    //use compact form or not

                    if (_useCompactInstruction)
                    {
                        //this is our extension,
                        //if you don't want compact version
                        //just use original

                        glyphData.GlyphInstructions = _instCompacter.Compact(instList.InnerInsts);

#if DEBUG
                        total += glyphData.GlyphInstructions.Length / (float)instList.InnerInsts.Count;
#endif
                    }
                    else
                    {
                        glyphData.GlyphInstructions = instList.InnerInsts.ToArray();
                    }
                }
                glyphs[i] = new Glyph(glyphData, (ushort)i);
            }

#if DEBUG
            if (_useCompactInstruction)
            {
                double avg = total / glyphCount;
                System.Diagnostics.Debug.WriteLine("cff instruction compact avg:" + avg + "%");
            }
#endif
        }

        //---------------
        private bool _useCompactInstruction = true;

        private Type2InstructionCompacter _instCompacter = new Type2InstructionCompacter();

        private void ReadFormat0Encoding()
        {
            //Table 11: Format 0
            //Type      Name            Description
            //Card8     format          = 0
            //Card8     nCodes          Number of encoded glyphs
            //Card8     code[nCodes]    Code array
            //-------
            //Each element of the code array represents the encoding for the
            //corresponding glyph.This format should be used when the
            //codes are in a fairly random order

            //we have read format field( 1st field) ..
            //so start with 2nd field

            int nCodes = _reader.ReadByte();
            byte[] codes = _reader.ReadBytes(nCodes);
        }

        private void ReadFormat1Encoding()
        {
            //Table 12 Format 1
            //Type      Name              Description
            //Card8     format             = 1
            //Card8     nRanges           Number of code ranges
            //struct    Range1[nRanges]   Range1 array(see Table  13)
            //--------------
            int nRanges = _reader.ReadByte();

            //Table 13 Range1 Format(Encoding)
            //Type        Name        Description
            //Card8       first       First code in range
            //Card8       nLeft       Codes left in range(excluding first)
            //--------------
            //Each Range1 describes a group of sequential codes. For
            //example, the codes 51 52 53 54 55 could be represented by the
            //Range1: 51 4, and a perfectly ordered encoding of 256 codes can
            //be described with the Range1: 0 255.

            //This format is particularly suited to encodings that are well ordered.

            //A few fonts have multiply - encoded glyphs which are not
            //supported directly by any of the above formats. This situation is
            //indicated by setting the high - order bit in the format byte and
            //supplementing the encoding, regardless of format type, as
            //shown in Table 14.

            //Table 14 Supplemental Encoding Data
            //Type 	    Name	    		Description
            //Card8	    nSups		    	Number of supplementary mappings
            //struct    Supplement[nSups]   Supplementary encoding array(see Table  15 below)

            //Table 15 Supplement Format
            //Type      Name        Description
            //Card8     code        Encoding
            //SID       glyph       Name
        }

        private int _privateDICTOffset;
        private int _privateDICTLen;

        private void ReadPrivateDict()
        {
            //per-font
            if (_privateDICTLen == 0) { return; }
            //
            _reader.BaseStream.Position = _cffStartAt + _privateDICTOffset;
            List<CffDataDicEntry> dicData = ReadDICTData(_privateDICTLen);

            if (dicData.Count > 0)
            {
                //interpret the values of private dict
                foreach (CffDataDicEntry dicEntry in dicData)
                {
                    switch (dicEntry._operator.Name)
                    {
                        case "Subrs":
                            {
                                var localSubrsOffset = (int)dicEntry.operands[0]._realNumValue;
                                _reader.BaseStream.Position = _cffStartAt + _privateDICTOffset + localSubrsOffset;
                                ReadLocalSubrs();
                            }
                            break;

                        case "defaultWidthX":
                            _currentCff1Font._defaultWidthX = (int)dicEntry.operands[0]._realNumValue;
                            break;

                        case "nominalWidthX":
                            _currentCff1Font._nominalWidthX = (int)dicEntry.operands[0]._realNumValue;
                            break;

                        default:
                            {
#if DEBUG
                                System.Diagnostics.Debug.WriteLine("cff_pri_dic:" + dicEntry._operator.Name);
#endif
                            }
                            break;
                    }
                }
            }
        }

        private List<byte[]> ReadSubrBuffer()
        {
            CffIndexOffset[] offsets = ReadIndexDataOffsets();
            if (offsets == null) return null;
            //
            int nsubrs = offsets.Length;
            List<byte[]> rawBufferList = new List<byte[]>();

            for (var i = 0; i < nsubrs; ++i)
            {
                CffIndexOffset offset = offsets[i];
                byte[] charStringBuffer = _reader.ReadBytes(offset.len);
                rawBufferList.Add(charStringBuffer);
            }
            return rawBufferList;
        }

        private List<CffDataDicEntry> ReadDICTData(int len)
        {
            //4. DICT Data

            //Font dictionary data comprising key-value pairs is represented
            //in a compact tokenized format that is similar to that used to
            //represent Type 1 charstrings.

            //Dictionary keys are encoded as 1- or 2-byte operators and dictionary values are encoded as
            //variable-size numeric operands that represent either integer or
            //real values.

            //-----------------------------
            //A DICT is simply a sequence of
            //operand(s)/operator bytes concatenated together.
            var endBefore = (int)(_reader.BaseStream.Position + len);
            List<CffDataDicEntry> dicData = new List<CffDataDicEntry>();
            while (_reader.BaseStream.Position < endBefore)
            {
                CffDataDicEntry dicEntry = ReadEntry();
                dicData.Add(dicEntry);
            }
            return dicData;
        }

        private CffDataDicEntry ReadEntry()
        {
            //-----------------------------
            //An operator is preceded by the operand(s) that
            //specify its value.
            //--------------------------------

            //-----------------------------
            //Operators and operands may be distinguished by inspection of
            //their first byte:
            //0–21 specify operators and
            //28, 29, 30, and 32–254 specify operands(numbers).
            //Byte values 22–27, 31, and 255 are reserved.

            //An operator may be preceded by up to a maximum of 48 operands

            var dicEntry = new CffDataDicEntry();
            var operands = new List<CffOperand>();

            while (true)
            {
                byte b0 = _reader.ReadByte();

                if (b0 >= 0 && b0 <= 21)
                {
                    //operators
                    dicEntry._operator = ReadOperator(b0);
                    break; //**break after found operator
                }
                else if (b0 == 28 || b0 == 29)
                {
                    int num = ReadIntegerNumber(b0);
                    operands.Add(new CffOperand(num, OperandKind.IntNumber));
                }
                else if (b0 == 30)
                {
                    double num = ReadRealNumber();
                    operands.Add(new CffOperand(num, OperandKind.RealNumber));
                }
                else if (b0 >= 32 && b0 <= 254)
                {
                    int num = ReadIntegerNumber(b0);
                    operands.Add(new CffOperand(num, OperandKind.IntNumber));
                }
                else
                {
                    throw new OpenFontNotSupportedException("invalid DICT data b0 byte: " + b0);
                }
            }

            dicEntry.operands = operands.ToArray();
            return dicEntry;
        }

        private CFFOperator ReadOperator(byte b0)
        {
            //read operator key
            byte b1 = 0;
            if (b0 == 12)
            {
                //2 bytes
                b1 = _reader.ReadByte();
            }
            //get registered operator by its key
            return CFFOperator.GetOperatorByKey(b0, b1);
        }

        private readonly StringBuilder _sbForReadRealNumber = new StringBuilder();

        private double ReadRealNumber()
        {
            //from https://typekit.files.wordpress.com/2013/05/5176.cff.pdf
            // A real number operand is provided in addition to integer
            //operands.This operand begins with a byte value of 30 followed
            //by a variable-length sequence of bytes.Each byte is composed
            //of two 4 - bit nibbles asdefined in Table 5.

            //The first nibble of a
            //pair is stored in the most significant 4 bits of a byte and the
            //second nibble of a pair is stored in the least significant 4 bits of a byte

            StringBuilder sb = _sbForReadRealNumber;
            sb.Length = 0;//reset

            var done = false;
            var exponentMissing = false;
            while (!done)
            {
                int b = _reader.ReadByte();

                int nb_0 = (b >> 4) & 0xf;
                int nb_1 = (b) & 0xf;

                for (var i = 0; !done && i < 2; ++i)
                {
                    int nibble = (i == 0) ? nb_0 : nb_1;

                    switch (nibble)
                    {
                        case 0x0:
                        case 0x1:
                        case 0x2:
                        case 0x3:
                        case 0x4:
                        case 0x5:
                        case 0x6:
                        case 0x7:
                        case 0x8:
                        case 0x9:
                            sb.Append(nibble);
                            exponentMissing = false;
                            break;

                        case 0xa:
                            sb.Append(".");
                            break;

                        case 0xb:
                            sb.Append("E");
                            exponentMissing = true;
                            break;

                        case 0xc:
                            sb.Append("E-");
                            exponentMissing = true;
                            break;

                        case 0xd:
                            break;

                        case 0xe:
                            sb.Append("-");
                            break;

                        case 0xf:
                            done = true;
                            break;

                        default:
                            throw new Exception("IllegalArgumentException");
                    }
                }
            }
            if (exponentMissing)
            {
                // the exponent is missing, just append "0" to avoid an exception
                // not sure if 0 is the correct value, but it seems to fit
                // see PDFBOX-1522
                sb.Append("0");
            }
            if (sb.Length == 0)
            {
                return 0d;
            }

            //TODO: use TryParse

            if (!double.TryParse(sb.ToString(),
                System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                System.Globalization.CultureInfo.InvariantCulture, out double value))
            {
                throw new OpenFontNotSupportedException();
            }
            return value;
        }

        private int ReadIntegerNumber(byte b0)
        {
            if (b0 == 28)
            {
                return _reader.ReadInt16();
            }
            else if (b0 == 29)
            {
                return _reader.ReadInt32();
            }
            else if (b0 >= 32 && b0 <= 246)
            {
                return b0 - 139;
            }
            else if (b0 >= 247 && b0 <= 250)
            {
                int b1 = _reader.ReadByte();
                return (b0 - 247) * 256 + b1 + 108;
            }
            else if (b0 >= 251 && b0 <= 254)
            {
                int b1 = _reader.ReadByte();
                return -(b0 - 251) * 256 - b1 - 108;
            }
            else
            {
                throw new Exception();
            }
        }

        private CffIndexOffset[] ReadIndexDataOffsets()
        {
            //INDEX Data
            //An INDEX is an array of variable-sized objects.It comprises a
            //header, an offset array, and object data.
            //The offset array specifies offsets within the object data.
            //An object is retrieved by
            //indexing the offset array and fetching the object at the
            //specified offset.
            //The object’s length can be determined by subtracting its offset
            //from the next offset in the offset array.
            //An additional offset is added at the end of the offset array so the
            //length of the last object may be determined.
            //The INDEX format is shown in Table 7

            //Table 7 INDEX Format
            //Type        Name                  Description
            //Card16      count                 Number of objects stored in INDEX
            //OffSize     offSize               Offset array element size
            //Offset      offset[count + 1]     Offset array(from byte preceding object data)
            //Card8       data[<varies>]        Object data

            //Offsets in the offset array are relative to the byte that precedes
            //the object data. Therefore the first element of the offset array
            //is always 1. (This ensures that every object has a corresponding
            //offset which is always nonzero and permits the efficient
            //implementation of dynamic object loading.)

            //An empty INDEX is represented by a count field with a 0 value
            //and no additional fields.Thus, the total size of an empty INDEX
            //is 2 bytes.

            //Note 2
            //An INDEX may be skipped by jumping to the offset specified by the last
            //element of the offset array

            ushort count = _reader.ReadUInt16();
            if (count == 0)
            {
                return null;
            }

            int offSize = _reader.ReadByte(); //
            var offsets = new int[count + 1];
            var indexElems = new CffIndexOffset[count];
            for (var i = 0; i <= count; ++i)
            {
                offsets[i] = _reader.ReadOffset(offSize);
            }
            for (var i = 0; i < count; ++i)
            {
                indexElems[i] = new CffIndexOffset(offsets[i], offsets[i + 1] - offsets[i]);
            }
            return indexElems;
        }

        private readonly struct CffIndexOffset
        {
            /// <summary>
            /// start offset
            /// </summary>
            private readonly int startOffset;

            public readonly int len;

            public CffIndexOffset(int startOffset, int len)
            {
                this.startOffset = startOffset;
                this.len = len;
            }

#if DEBUG

            public override string ToString()
            {
                return "offset:" + startOffset + ",len:" + len;
            }

#endif
        }
    }
}
