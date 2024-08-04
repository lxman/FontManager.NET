﻿using System;
using System.IO;
using FontParser.Tables.BitmapAndSvgFonts.Common;
using FontParser.Tables.BitmapAndSvgFonts.Common.SubTables;

namespace FontParser.Tables.BitmapAndSvgFonts
{
    /// <summary>
    /// EBLC : Embedded bitmap location data
    /// </summary>
    internal class EBLC : TableEntry
    {
        public const string _N = "EBLC";
        public override string Name => _N;
        //
        //from https://docs.microsoft.com/en-us/typography/opentype/spec/eblc
        //EBLC - Embedded Bitmap Location Table
        //----------------------------------------------
        //The EBLC provides embedded bitmap locators.It is used together with the EDBTtable, which provides embedded, monochrome or grayscale bitmap glyph data, and the EBSC table, which provided embedded bitmap scaling information.
        //OpenType embedded bitmaps are called 'sbits' (for “scaler bitmaps”). A set of bitmaps for a face at a given size is called a strike.
        //The 'EBLC' table identifies the sizes and glyph ranges of the sbits, and keeps offsets to glyph bitmap data in indexSubTables.The 'EBDT' table then stores the glyph bitmap data, also in a number of different possible formats.Glyph metrics information may be stored in either the 'EBLC' or 'EBDT' table, depending upon the indexSubTable and glyph bitmap formats. The 'EBSC' table identifies sizes that will be handled by scaling up or scaling down other sbit sizes.
        //The 'EBLC' table uses the same format as the Apple Advanced Typography (AAT) 'bloc' table.
        //The 'EBLC' table begins with a header containing the table version and number of strikes.An OpenType font may have one or more strikes embedded in the 'EBDT' table.
        //----------------------------------------------
        //eblcHeader
        //----------------------------------------------
        //Type      Name            Description
        //uint16    majorVersion    Major version of the EBLC table, = 2.
        //uint16    minorVersion    Minor version of the EBLC table, = 0.
        //uint32    numSizes        Number of bitmapSizeTables
        //----------------------------------------------
        //Note that the first version of the EBLC table is 2.0.
        //The eblcHeader is followed immediately by the bitmapSizeTable array(s).
        //The numSizes in the eblcHeader indicates the number of bitmapSizeTables in the array.
        //Each strike is defined by one bitmapSizeTable.

        private SizeTable[] _bmpSizeTables;

        protected override void ReadContentFrom(BinaryReader reader)
        {
            // load each strike table
            long eblcBeginPos = reader.BaseStream.Position;
            //
            ushort versionMajor = reader.ReadUInt16();
            ushort versionMinor = reader.ReadUInt16();
            uint numSizes = reader.ReadUInt32();

            if (numSizes > MAX_BITMAP_STRIKES)
                throw new Exception("Too many bitmap strikes in font.");

            //----------------
            var bmpSizeTables = new SizeTable[numSizes];
            for (int i = 0; i < numSizes; i++)
            {
                bmpSizeTables[i] = SizeTable.ReadBitmapSizeTable(reader);
            }
            _bmpSizeTables = bmpSizeTables;

            //
            //-------
            //IndexSubTableArray
            //Type      Name            Description
            //uint16    firstGlyphIndex First glyph ID of this range.
            //uint16    lastGlyphIndex  Last glyph ID of this range(inclusive).
            //Offset32  additionalOffsetToIndexSubtable     Add to indexSubTableArrayOffset to get offset from beginning of EBLC.

            //After determining the strike,
            //the rasterizer searches this array for the range containing the given glyph ID.
            //When the range is found, the additionalOffsetToIndexSubtable is added to the indexSubTableArrayOffset
            //to get the offset of the IndexSubTable in the EBLC.

            //The first indexSubTableArray is located after the last bitmapSizeSubTable entry.
            //Then the IndexSubTables for the strike follow.
            //Another IndexSubTableArray(if more than one strike) and
            //its IndexSubTableArray are next.

            //The EBLC continues with an array and IndexSubTables for each strike.
            //We now have the offset to the IndexSubTable.
            //All IndexSubTable formats begin with an IndexSubHeader which identifies the IndexSubTable format,
            //the format of the EBDT image data,
            //and the offset from the beginning of the EBDT table to the beginning of the image data for this range.

            for (int n = 0; n < numSizes; ++n)
            {
                SizeTable bmpSizeTable = bmpSizeTables[n];
                uint numberOfIndexSubTables = bmpSizeTable.numberOfIndexSubTables;

                //
                IndexSubTableArray[] indexSubTableArrs = new IndexSubTableArray[numberOfIndexSubTables];
                for (uint i = 0; i < numberOfIndexSubTables; ++i)
                {
                    indexSubTableArrs[i] = new IndexSubTableArray(
                             reader.ReadUInt16(), //First glyph ID of this range.
                             reader.ReadUInt16(), //Last glyph ID of this range (inclusive).
                             reader.ReadUInt32());//Add to indexSubTableArrayOffset to get offset from beginning of EBLC.
                }

                //---
                IndexSubTableBase[] subTables = new IndexSubTableBase[numberOfIndexSubTables];
                bmpSizeTable.indexSubTables = subTables;
                for (uint i = 0; i < numberOfIndexSubTables; ++i)
                {
                    IndexSubTableArray indexSubTableArr = indexSubTableArrs[i];
                    reader.BaseStream.Position = eblcBeginPos + bmpSizeTable.indexSubTableArrayOffset + indexSubTableArr.additionalOffsetToIndexSubtable;

                    subTables[i] = IndexSubTableBase.CreateFrom(bmpSizeTable, reader);
                }
            }
        }

        private const int MAX_BITMAP_STRIKES = 1024;
    }
}