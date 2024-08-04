using System.Collections.Generic;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    public abstract class IndexSubTableBase
    {
        public IndexSubHeader header;

        public abstract int SubTypeNo { get; }
        public ushort firstGlyphIndex;
        public ushort lastGlyphIndex;

        public static IndexSubTableBase CreateFrom(SizeTable bmpSizeTable, BinaryReader reader)
        {
            //read IndexSubHeader
            //IndexSubHeader
            //Type      Name            Description
            //uint16    indexFormat     Format of this IndexSubTable.
            //uint16    imageFormat     Format of EBDT image data.
            //Offset32  imageDataOffset Offset to image data in EBDT table.

            //There are currently five different formats used for the IndexSubTable,
            //depending upon the size and type of bitmap data in the glyph ID range.

            //Apple 'bloc' tables support only formats 1 through 3.

            //The choice of which IndexSubTable format to use is up to the font manufacturer,
            //but should be made with the aim of minimizing the size of the font file.
            //Ranges of glyphs with variable metrics — that is,
            //where glyphs may differ from each other in bounding box height, width, side bearings or
            //advance — must use format 1, 3 or 4.

            //Ranges of glyphs with constant metrics can save space by using format 2 or 5,
            //which keep a single copy of the metrics information in the IndexSubTable rather
            //than a copy per glyph in the EBDT table.

            //In some monospaced fonts it makes sense to store extra white space around
            //some of the glyphs to keep all metrics identical, thus permitting the use of format 2 or 5.

            IndexSubHeader header = new IndexSubHeader(
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadUInt32()
                );

            switch (header.indexFormat)
            {
                case 1:

                    //IndexSubTable1: variable - metrics glyphs with 4 - byte offsets
                    //Type                  Name            Description
                    //IndexSubHeader        header          Header info.
                    //Offset32              offsetArray[]   offsetArray[glyphIndex] + imageDataOffset = glyphData sizeOfArray = (lastGlyph - firstGlyph + 1) + 1 + 1 pad if needed
                    {
                        int nElem = bmpSizeTable.endGlyphIndex - bmpSizeTable.startGlyphIndex + 1;
                        uint[] offsetArray = reader.ReadUInt32Array(nElem);
                        //check 16 bit align padd
                        IndexSubTable1 subTable = new IndexSubTable1
                        {
                            header = header,
                            offsetArray = offsetArray
                        };
                        return subTable;
                    }
                case 2:
                    //IndexSubTable2: all glyphs have identical metrics
                    //Type                 Name Description
                    //IndexSubHeader       header  Header info.
                    //uint32               imageSize   All the glyphs are of the same size.
                    //BigGlyphMetrics      bigMetrics  All glyphs have the same metrics; glyph data may be compressed, byte-aligned, or bit-aligned.
                    {
                        IndexSubTable2 subtable = new IndexSubTable2
                        {
                            header = header,
                            imageSize = reader.ReadUInt32()
                        };
                        BigGlyphMetrics.ReadBigGlyphMetric(reader, ref subtable.BigGlyphMetrics);
                        return subtable;
                    }

                case 3:
                    //IndexSubTable3: variable - metrics glyphs with 2 - byte offsets
                    //Type                 Name         Description
                    //IndexSubHeader       header       Header info.
                    //Offset16             offsetArray[]   offsetArray[glyphIndex] + imageDataOffset = glyphData sizeOfArray = (lastGlyph - firstGlyph + 1) + 1 + 1 pad if needed
                    {
                        int nElem = bmpSizeTable.endGlyphIndex - bmpSizeTable.startGlyphIndex + 1;
                        ushort[] offsetArray = reader.ReadUInt16Array(nElem);
                        //check 16 bit align padd
                        IndexSubTable3 subTable = new IndexSubTable3
                        {
                            header = header,
                            offsetArray = offsetArray
                        };
                        return subTable;
                    }
                case 4:
                    //IndexSubTable4: variable - metrics glyphs with sparse glyph codes
                    //Type                Name      Description
                    //IndexSubHeader      header    Header info.
                    //uint32              numGlyphs Array length.
                    //GlyphIdOffsetPair   glyphArray[numGlyphs + 1]   One per glyph.
                    {
                        IndexSubTable4 subTable = new IndexSubTable4
                        {
                            header = header
                        };

                        uint numGlyphs = reader.ReadUInt32();
                        GlyphIdOffsetPair[] glyphArray = subTable.glyphArray = new GlyphIdOffsetPair[numGlyphs + 1];
                        for (int i = 0; i <= numGlyphs; ++i) //***
                        {
                            glyphArray[i] = new GlyphIdOffsetPair(reader.ReadUInt16(), reader.ReadUInt16());
                        }
                        return subTable;
                    }
                case 5:
                    //IndexSubTable5: constant - metrics glyphs with sparse glyph codes
                    //Type                Name     Description
                    //IndexSubHeader      header  Header info.
                    //uint32              imageSize   All glyphs have the same data size.
                    //BigGlyphMetrics     bigMetrics  All glyphs have the same metrics.
                    //uint32              numGlyphs   Array length.
                    //uint16              glyphIdArray[numGlyphs]     One per glyph, sorted by glyph ID.
                    {
                        IndexSubTable5 subTable = new IndexSubTable5
                        {
                            header = header,
                            imageSize = reader.ReadUInt32()
                        };

                        BigGlyphMetrics.ReadBigGlyphMetric(reader, ref subTable.BigGlyphMetrics);
                        subTable.glyphIdArray = reader.ReadUInt16Array((int)reader.ReadUInt32());
                        return subTable;
                    }
            }

            //The size of the EBDT image data can be calculated from the IndexSubTable information.
            //For the constant-metrics formats(2 and 5) the image data size is constant,
            //and is given in the imageSize field.For the variable metrics formats(1, 3, and 4)
            //image data must be stored contiguously and in glyph ID order,
            //so the image data size may be calculated by subtracting the offset for
            //the current glyph from the offset of the next glyph.

            //Because of this, it is necessary to store one extra element in the offsetArray pointing
            //just past the end of the range’s image data.
            //This will allow the correct calculation of the image data size for the last glyph in the range.

            //Contiguous, or nearly contiguous,
            //ranges of glyph IDs are handled best by formats 1, 2, and 3 which
            //store an offset for every glyph ID in the range.
            //Very sparse ranges of glyph IDs should use format 4 or 5 which explicitly
            //call out the glyph IDs represented in the range.
            //A small number of missing glyphs can be efficiently represented in formats 1 or 3 by having
            //the offset for the missing glyph be followed by the same offset for
            //the next glyph, thus indicating a data size of zero.

            //The only difference between formats 1 and 3 is
            //the size of the offsetArray elements: format 1 uses uint32s while format 3 uses uint16s.
            //Therefore format 1 can cover a greater range(> 64k bytes)
            //while format 3 saves more space in the EBLC table.
            //Since the offsetArray elements are added to the imageDataOffset base address in the IndexSubHeader,
            //a very large set of glyph bitmap data could be addressed by splitting it into multiple ranges,
            //each less than 64k bytes in size,
            //allowing the use of the more efficient format 3.

            //The EBLC table specification requires 16 - bit alignment for all subtables.
            //This occurs naturally for IndexSubTable formats 1, 2, and 4,
            //but may not for formats 3 and 5,
            //since they include arrays of type uint16.
            //When there is an odd number of elements in these arrays
            //**it is necessary to add an extra padding element to maintain proper alignment.

            return null;
        }

        public abstract void BuildGlyphList(List<Glyph> glyphList);
    }
}
