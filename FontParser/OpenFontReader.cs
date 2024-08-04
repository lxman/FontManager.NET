using System.IO;
using FontParser.AdditionalInfo;
using FontParser.Exceptions;
using FontParser.Tables;
using FontParser.Tables.AdvancedLayout;
using FontParser.Tables.AdvancedLayout.Base;
using FontParser.Tables.AdvancedLayout.FontMath;
using FontParser.Tables.AdvancedLayout.GPOS;
using FontParser.Tables.AdvancedLayout.GSUB;
using FontParser.Tables.AdvancedLayout.JustificationTable;
using FontParser.Tables.BitmapAndSvgFonts;
using FontParser.Tables.CFF;
using FontParser.Tables.Others;
using FontParser.Tables.TrueType;
using FontParser.Tables.Variations;
using FontParser.Typeface;
using FontParser.Typeface.Os2;
using Kern = FontParser.Tables.Others.Kern;

namespace FontParser
{
    public class OpenFontReader
    {
        private static string BuildTtcfName(PreviewFontInfo[] members)
        {
            //THIS IS MY CONVENTION for TrueType collection font name
            //you can change this to fit your need.

            var stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("TTCF: " + members.Length);
            var uniqueNames = new System.Collections.Generic.Dictionary<string, bool>();
            for (uint i = 0; i < members.Length; ++i)
            {
                PreviewFontInfo member = members[i];
                if (uniqueNames.TryAdd(member.Name, true))
                {
                    stringBuilder.Append("," + member.Name);
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// read only name entry
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public PreviewFontInfo ReadPreview(Stream stream)
        {
            //var little = BitConverter.IsLittleEndian;
            using var input = new ByteOrderSwappingBinaryReader(stream);
            ushort majorVersion = input.ReadUInt16();
            ushort minorVersion = input.ReadUInt16();

            if (KnownFontFiles.IsTtcf(majorVersion, minorVersion))
            {
                //this font stream is 'The Font Collection'
                FontCollectionHeader ttcHeader = ReadTtcHeader(input);
                PreviewFontInfo[] members = new PreviewFontInfo[ttcHeader.numFonts];
                for (uint i = 0; i < ttcHeader.numFonts; ++i)
                {
                    input.BaseStream.Seek(ttcHeader.offsetTables[i], SeekOrigin.Begin);
                    PreviewFontInfo member = members[i] = ReadActualFontPreview(input, false);
                    member.ActualStreamOffset = ttcHeader.offsetTables[i];
                }
                return new PreviewFontInfo(BuildTtcfName(members), members);
            }
            else if (KnownFontFiles.IsWoff(majorVersion, minorVersion))
            {
                //check if we enable woff or not
                WebFont.WoffReader woffReader = new WebFont.WoffReader();
                input.BaseStream.Position = 0;
                return woffReader.ReadPreview(input);
            }
            else if (KnownFontFiles.IsWoff2(majorVersion, minorVersion))
            {
                //check if we enable woff2 or not
                WebFont.Woff2Reader woffReader = new WebFont.Woff2Reader();
                input.BaseStream.Position = 0;
                return woffReader.ReadPreview(input);
            }
            else
            {
                return ReadActualFontPreview(input, true);//skip version data (majorVersion, minorVersion)
            }
        }

        private FontCollectionHeader ReadTtcHeader(ByteOrderSwappingBinaryReader input)
        {
            //https://docs.microsoft.com/en-us/typography/opentype/spec/otff#ttc-header
            //TTC Header Version 1.0:
            //Type 	    Name 	        Description
            //TAG 	    ttcTag 	        Font Collection ID string: 'ttcf' (used for fonts with CFF or CFF2 outlines as well as TrueType outlines)
            //uint16 	majorVersion 	Major version of the TTC Header, = 1.
            //uint16 	minorVersion 	Minor version of the TTC Header, = 0.
            //uint32 	numFonts 	    Number of fonts in TTC
            //Offset32 	offsetTable[numFonts] 	Array of offsets to the OffsetTable for each font from the beginning of the file

            //TTC Header Version 2.0:
            //Type 	    Name 	        Description
            //TAG 	    ttcTag 	        Font Collection ID string: 'ttcf'
            //uint16 	majorVersion 	Major version of the TTC Header, = 2.
            //uint16 	minorVersion 	Minor version of the TTC Header, = 0.
            //uint32 	numFonts 	    Number of fonts in TTC
            //Offset32 	offsetTable[numFonts] 	Array of offsets to the OffsetTable for each font from the beginning of the file
            //uint32 	dsigTag 	    Tag indicating that a DSIG table exists, 0x44534947 ('DSIG') (null if no signature)
            //uint32 	dsigLength 	    The length (in bytes) of the DSIG table (null if no signature)
            //uint32 	dsigOffset 	    The offset (in bytes) of the DSIG table from the beginning of the TTC file (null if no signature)

            var ttcHeader = new FontCollectionHeader
            {
                majorVersion = input.ReadUInt16(),
                minorVersion = input.ReadUInt16()
            };

            uint numFonts = input.ReadUInt32();
            int[] offsetTables = new int[numFonts];
            for (uint i = 0; i < numFonts; ++i)
            {
                offsetTables[i] = input.ReadInt32();
            }

            ttcHeader.numFonts = numFonts;
            ttcHeader.offsetTables = offsetTables;
            //
            if (ttcHeader.majorVersion == 2)
            {
                ttcHeader.dsigTag = input.ReadUInt32();
                ttcHeader.dsigLength = input.ReadUInt32();
                ttcHeader.dsigOffset = input.ReadUInt32();

                if (ttcHeader.dsigTag == 0x44534947)
                {
                    //Tag indicating that a DSIG table exists
                    //TODO: goto DSIG add read signature
                }
            }
            return ttcHeader;
        }

        private PreviewFontInfo ReadActualFontPreview(ByteOrderSwappingBinaryReader input, bool skipVersionData)
        {
            if (!skipVersionData)
            {
                ushort majorVersion = input.ReadUInt16();
                ushort minorVersion = input.ReadUInt16();
            }

            ushort tableCount = input.ReadUInt16();
            ushort searchRange = input.ReadUInt16();
            ushort entrySelector = input.ReadUInt16();
            ushort rangeShift = input.ReadUInt16();

            var tables = new TableEntryCollection();
            for (int i = 0; i < tableCount; i++)
            {
                tables.AddEntry(new UnreadTableEntry(ReadTableHeader(input)));
            }
            return ReadPreviewFontInfo(tables, input);
        }

        public Typeface.Typeface Read(Stream stream, int streamStartOffset = 0, ReadFlags readFlags = ReadFlags.Full)
        {
            Typeface.Typeface typeface = new Typeface.Typeface();
            if (Read(typeface, null, stream, streamStartOffset, readFlags))
            {
                return typeface;
            }
            return null;
        }

        public bool Read(Typeface.Typeface typeface, RestoreTicket ticket, Stream stream, int streamStartOffset = 0, ReadFlags readFlags = ReadFlags.Full)
        {
            if (streamStartOffset > 0)
            {
                //eg. for ttc
                stream.Seek(streamStartOffset, SeekOrigin.Begin);
            }
            using var input = new ByteOrderSwappingBinaryReader(stream);
            ushort majorVersion = input.ReadUInt16();
            ushort minorVersion = input.ReadUInt16();

            if (KnownFontFiles.IsTtcf(majorVersion, minorVersion))
            {
                //this font stream is 'The Font Collection'
                //To read content of ttc=> one must specific the offset
                //so use read preview first=> you will know that what are inside the ttc.
                return false;
            }
            else if (KnownFontFiles.IsWoff(majorVersion, minorVersion))
            {
                //check if we enable woff or not
                WebFont.WoffReader woffReader = new WebFont.WoffReader();
                input.BaseStream.Position = 0;
                return woffReader.Read(typeface, input, ticket);
            }
            else if (KnownFontFiles.IsWoff2(majorVersion, minorVersion))
            {
                //check if we enable woff2 or not
                WebFont.Woff2Reader woffReader = new WebFont.Woff2Reader();
                input.BaseStream.Position = 0;
                return woffReader.Read(typeface, input, ticket);
            }
            //-----------------------------------------------------------------

            ushort tableCount = input.ReadUInt16();
            ushort searchRange = input.ReadUInt16();
            ushort entrySelector = input.ReadUInt16();
            ushort rangeShift = input.ReadUInt16();
            //------------------------------------------------------------------
            var tables = new TableEntryCollection();
            for (int i = 0; i < tableCount; i++)
            {
                tables.AddEntry(new UnreadTableEntry(ReadTableHeader(input)));
            }
            //------------------------------------------------------------------

            return ReadTableEntryCollection(typeface, ticket, tables, input);
        }

        public PreviewFontInfo ReadPreviewFontInfo(TableEntryCollection tables, BinaryReader input)
        {
            var rd = new EntriesReaderHelper(tables, input);

            NameEntry nameEntry = rd.Read(new NameEntry());
            Os2Table os2Table = rd.Read(new Os2Table());
            //for preview, read ONLY  script list from gsub and gpos (set OnlyScriptList).
            Meta metaTable = rd.Read(new Meta());

            GSUB gsub = rd.Read(new GSUB() { OnlyScriptList = true });
            GPOS gpos = rd.Read(new GPOS() { OnlyScriptList = true });
            Cmap cmap = rd.Read(new Cmap());
            //gsub and gpos contains actual script_list that are in the typeface

            Languages langs = new Languages();
            langs.Update(os2Table, metaTable, cmap, gsub, gpos);

            return new PreviewFontInfo(
              nameEntry,
              os2Table,
              langs);
        }

        private static bool ReadTableEntryCollectionOnRestoreMode(Typeface.Typeface typeface, RestoreTicket ticket, TableEntryCollection tables, BinaryReader input)
        {
            //RESTORE MODE
            //check header matches

            if (!typeface.IsTrimmed() ||
                !typeface.CompareOriginalHeadersWithNewlyLoadOne(tables.CloneTableHeaders()))
            {
                return false;
            }

            var rd = new EntriesReaderHelper(tables, input);
            //PART 1: basic information
            //..
            //------------------------------------
            //PART 2: glyphs detail
            //2.1 True type font
            GlyphLocations glyphLocations = ticket.HasTtf ? rd.Read(new GlyphLocations(typeface.MaxProfile.GlyphCount, typeface.Head.WideGlyphLocations)) : null;
            Glyf glyf = ticket.HasTtf ? rd.Read(new Glyf(glyphLocations)) : null;

            typeface.GaspTable = ticket.GaspTable ? rd.Read(new Gasp()) : null;

            typeface.SetColorAndPalTable(
                ticket.COLRTable ? rd.Read(new COLR()) : null,
                ticket.CPALTable ? rd.Read(new CPAL()) : null);

            //2.2 Cff font
            CFFTable cff = ticket.HasCff ? rd.Read(new CFFTable()) : null;

            bool isPostScriptOutline = false;
            bool isBitmapFont = false;

            if (glyf == null)
            {
                //check if this is cff table ?
                if (cff == null)
                {
                    //check  cbdt/cblc ?
                    if (ticket.HasBitmapSource)
                    {
                        //reload only CBDT (embeded bitmap)
                        CBDT cbdt = rd.Read(new CBDT());
                        typeface._bitmapFontGlyphSource.LoadCBDT(cbdt);
                        //just clone existing glyph
                        isBitmapFont = true;
                    }
                    else
                    {
                        //?
                        throw new OpenFontNotSupportedException();
                    }
                }
                else
                {
                    isPostScriptOutline = true;
                    typeface.SetCffFontSet(cff.Cff1FontSet);
                }
            }
            else
            {
                typeface.SetTtfGlyphs(glyf.Glyphs);
            }

            if (!isPostScriptOutline && !isBitmapFont)
            {
                //for true-type font outline
                FpgmTable fpgmTable = rd.Read(new FpgmTable());
                //control values table
                CvtTable cvtTable = rd.Read(new CvtTable());
                PrepTable propProgramTable = rd.Read(new PrepTable());

                typeface.ControlValues = cvtTable?._controlValues;
                typeface.FpgmProgramBuffer = fpgmTable?._programBuffer;
                typeface.PrepProgramBuffer = propProgramTable?._programBuffer;
            }

            if (ticket.HasSvg)
            {
                typeface.SetSvgTable(rd.Read(new SvgTable()));
            }

#if DEBUG
            //test
            //int found = typeface.GetGlyphIndexByName("Uacute");
            if (typeface.IsCffFont)
            {
                //optional???
                typeface.UpdateAllCffGlyphBounds();
            }
#endif
            typeface._typefaceTrimMode = TrimMode.Restored;
            return true;
        }

        public bool ReadTableEntryCollection(Typeface.Typeface typeface, RestoreTicket ticket, TableEntryCollection tables, BinaryReader input)
        {
            if (ticket != null)
            {
                return ReadTableEntryCollectionOnRestoreMode(typeface, ticket, tables, input);
            }

            typeface.SetTableEntryCollection(tables.CloneTableHeaders());

            var rd = new EntriesReaderHelper(tables, input);
            //PART 1: basic information
            Os2Table os2Table = rd.Read(new Os2Table());
            Meta meta = rd.Read(new Meta());
            NameEntry nameEntry = rd.Read(new NameEntry());
            Head head = rd.Read(new Head());
            MaxProfile maxProfile = rd.Read(new MaxProfile());
            HorizontalHeader horizontalHeader = rd.Read(new HorizontalHeader());
            HorizontalMetrics horizontalMetrics = rd.Read(new HorizontalMetrics(horizontalHeader.NumberOfHMetrics, maxProfile.GlyphCount));
            VerticalHeader vhea = rd.Read(new VerticalHeader());
            if (vhea != null)
            {
                VerticalMetrics vmtx = rd.Read(new VerticalMetrics(vhea.NumOfLongVerMetrics));
            }

            Os2FsSelection os2Select = new Os2FsSelection(os2Table.fsSelection);
            typeface._useTypographicMertic = os2Select.USE_TYPO_METRICS;

            Cmap cmaps = rd.Read(new Cmap());
            VerticalDeviceMetrics vdmx = rd.Read(new VerticalDeviceMetrics());
            Kern kern = rd.Read(new Kern());
            //------------------------------------
            //PART 2: glyphs detail
            //2.1 True type font

            GlyphLocations glyphLocations = rd.Read(new GlyphLocations(maxProfile.GlyphCount, head.WideGlyphLocations));
            Glyf glyf = rd.Read(new Glyf(glyphLocations));
            Gasp gaspTable = rd.Read(new Gasp());
            COLR colr = rd.Read(new COLR());
            CPAL cpal = rd.Read(new CPAL());

            //2.2 Cff font
            PostTable postTable = rd.Read(new PostTable());
            CFFTable cff = rd.Read(new CFFTable());

            //additional math table (if available)
            MathTable mathtable = rd.Read(new MathTable());
            //------------------------------------

            //PART 3: advanced typography
            GDEF gdef = rd.Read(new GDEF());
            GSUB gsub = rd.Read(new GSUB());
            GPOS gpos = rd.Read(new GPOS());
            BASE baseTable = rd.Read(new BASE());
            JSTF jstf = rd.Read(new JSTF());

            STAT stat = rd.Read(new STAT());
            if (stat != null)
            {
                //variable font
                FVar fvar = rd.Read(new FVar());
                if (fvar != null)
                {
                    GVar gvar = rd.Read(new GVar());
                    CVar cvar = rd.Read(new CVar());
                    HVar hvar = rd.Read(new HVar());
                    MVar mvar = rd.Read(new MVar());
                    AVar avar = rd.Read(new AVar());
                }
            }

            bool isPostScriptOutline = false;
            bool isBitmapFont = false;

            typeface.SetBasicTypefaceTables(os2Table, nameEntry, head, horizontalMetrics);
            if (glyf == null)
            {
                //check if this is cff table ?
                if (cff == null)
                {
                    //check  cbdt/cblc ?
                    CBLC cblcTable = rd.Read(new CBLC());
                    if (cblcTable != null)
                    {
                        CBDT cbdtTable = rd.Read(new CBDT());
                        //read cbdt
                        //bitmap font

                        BitmapFontGlyphSource bmpFontGlyphSrc = new BitmapFontGlyphSource(cblcTable);
                        bmpFontGlyphSrc.LoadCBDT(cbdtTable);
                        Glyph[] glyphs = bmpFontGlyphSrc.BuildGlyphList();
                        typeface.SetBitmapGlyphs(glyphs, bmpFontGlyphSrc);
                        isBitmapFont = true;
                    }
                    else
                    {
                        //TODO:
                        EBLC fontBmpTable = rd.Read(new EBLC());
                        throw new OpenFontNotSupportedException();
                    }
                }
                else
                {
                    isPostScriptOutline = true;
                    typeface.SetCffFontSet(cff.Cff1FontSet);
                }
            }
            else
            {
                typeface.SetTtfGlyphs(glyf.Glyphs);
            }

            //----------------------------
            typeface.CmapTable = cmaps;
            typeface.KernTable = kern;
            typeface.MaxProfile = maxProfile;
            typeface.HheaTable = horizontalHeader;
            //----------------------------
            typeface.GaspTable = gaspTable;

            if (!isPostScriptOutline && !isBitmapFont)
            {
                //for true-type font outline
                FpgmTable fpgmTable = rd.Read(new FpgmTable());
                //control values table
                CvtTable cvtTable = rd.Read(new CvtTable());
                PrepTable propProgramTable = rd.Read(new PrepTable());

                typeface.ControlValues = cvtTable?._controlValues;
                typeface.FpgmProgramBuffer = fpgmTable?._programBuffer;
                typeface.PrepProgramBuffer = propProgramTable?._programBuffer;
            }

            //-------------------------
            typeface.LoadOpenFontLayoutInfo(
                gdef,
                gsub,
                gpos,
                baseTable,
                colr,
                cpal);
            //------------

            typeface.SetSvgTable(rd.Read(new SvgTable()));
            typeface.PostTable = postTable;

            if (mathtable != null)
            {
                GlyphLoader.LoadMathGlyph(typeface, mathtable);
            }
#if DEBUG
            //test
            //int found = typeface.GetGlyphIndexByName("Uacute");
            if (typeface.IsCffFont)
            {
                //optional
                typeface.UpdateAllCffGlyphBounds();
            }
#endif
            typeface.UpdateLangs(meta);
            typeface.UpdateFrequentlyUsedValues();
            return true;
        }

        private static TableHeader ReadTableHeader(BinaryReader input)
        {
            return new TableHeader(
                input.ReadUInt32(),
                input.ReadUInt32(),
                input.ReadUInt32(),
                input.ReadUInt32());
        }
    }
}