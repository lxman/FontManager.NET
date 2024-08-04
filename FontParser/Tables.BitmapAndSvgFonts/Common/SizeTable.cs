using System.IO;
using FontParser.Tables.BitmapAndSvgFonts.Common.SubTables;

namespace FontParser.Tables.BitmapAndSvgFonts.Common
{
    public class SizeTable
    {
        public uint indexSubTableArrayOffset;
        public uint indexTablesSize;
        public uint numberOfIndexSubTables;
        public uint colorRef;

        public SbitLineMetrics hori;
        public SbitLineMetrics vert;

        public ushort startGlyphIndex;
        public ushort endGlyphIndex;

        public byte ppemX;
        public byte ppemY;
        public byte bitDepth;

        //bitDepth
        //Value   Description
        //1	      black/white
        //2	      4 levels of gray
        //4	      16 levels of gray
        //8	      256 levels of gray

        public sbyte flags;

        //-----
        //reconstructed
        public IndexSubTableBase[] indexSubTables;

        //
        private static void ReadSbitLineMetrics(BinaryReader reader, ref SbitLineMetrics lineMetric)
        {
            //read 12 bytes ...

            lineMetric.ascender = (sbyte)reader.ReadByte();
            lineMetric.descender = (sbyte)reader.ReadByte();
            lineMetric.widthMax = reader.ReadByte();

            lineMetric.caretSlopeNumerator = (sbyte)reader.ReadByte();
            lineMetric.caretSlopeDenominator = (sbyte)reader.ReadByte();
            lineMetric.caretOffset = (sbyte)reader.ReadByte();

            lineMetric.minOriginSB = (sbyte)reader.ReadByte();
            lineMetric.minAdvanceSB = (sbyte)reader.ReadByte();

            lineMetric.maxBeforeBL = (sbyte)reader.ReadByte();
            lineMetric.minAfterBL = (sbyte)reader.ReadByte();

            lineMetric.pad1 = (sbyte)reader.ReadByte();
            lineMetric.pad2 = (sbyte)reader.ReadByte();
        }

        public static SizeTable ReadBitmapSizeTable(BinaryReader reader)
        {
            //EBLC's BitmapSize Table   (https://docs.microsoft.com/en-us/typography/opentype/spec/eblc)
            //Type          Name                        Description
            //Offset32      indexSubTableArrayOffset    Offset to IndexSubtableArray, from beginning of EBLC.
            //uint32        indexTablesSize             Number of bytes in corresponding index subtables and array.
            //uint32        numberOfIndexSubTables      There is an IndexSubtable for each range or format change.
            //uint32        colorRef                    Not used; set to 0.
            //SbitLineMetrics    hori                   Line metrics for text rendered horizontally.
            //SbitLineMetrics    vert                   Line metrics for text rendered vertically.
            //uint16            startGlyphIndex         Lowest glyph index for this size.
            //uint16            endGlyphIndex           Highest glyph index for this size.
            //uint8             ppemX                   Horizontal pixels per em.
            //uint8             ppemY                   Vertical pixels per em.
            //uint8             bitDepth                The Microsoft rasterizer v.1.7 or greater supports the following bitDepth values, as described below: 1, 2, 4, and 8.
            //int8              flags                   Vertical or horizontal(see Bitmap Flags, below).

            //CBLC's BitmapSize Table  (https://docs.microsoft.com/en-us/typography/opentype/spec/cblc)
            //Type                Name                      Description
            //Offset32            indexSubTableArrayOffset  Offset to index subtable from beginning of CBLC.
            //uint32              indexTablesSize           Number of bytes in corresponding index subtables and array.
            //uint32              numberOfIndexSubTables    There is an index subtable for each range or format change.
            //uint32              colorRef                  Not used; set to 0.
            //SbitLineMetrics     hori                      Line metrics for text rendered horizontally.
            //SbitLineMetrics     vert                      Line metrics for text rendered vertically.
            //uint16              startGlyphIndex           Lowest glyph index for this size.
            //uint16              endGlyphIndex             Highest glyph index for this size.
            //uint8               ppemX                     Horizontal pixels per em.
            //uint8               ppemY                     Vertical pixels per em.
            //uint8               bitDepth                  In addition to already defined bitDepth values 1, 2, 4, and 8
            //                                              supported by existing implementations, the value of 32 is used to
            //                                              identify color bitmaps with 8 bit per pixel RGBA channels.
            //int8                flags                     Vertical or horizontal(see the Bitmap Flags section of the EBLC table chapter).

            //The indexSubTableArrayOffset is the offset from the beginning of
            //the CBLC table to the indexSubTableArray.

            //Each strike has one of these arrays to support various formats and
            //discontinuous ranges of bitmaps.The indexTablesSize is
            //the total number of bytes in the indexSubTableArray and
            //the associated indexSubTables.
            //The numberOfIndexSubTables is a count of the indexSubTables for this strike.

            //The rest of the CBLC table structure is identical to one already defined for EBLC.

            SizeTable bmpSizeTable = new SizeTable
            {
                indexSubTableArrayOffset = reader.ReadUInt32(),
                indexTablesSize = reader.ReadUInt32(),
                numberOfIndexSubTables = reader.ReadUInt32(),
                colorRef = reader.ReadUInt32()
            };

            ReadSbitLineMetrics(reader, ref bmpSizeTable.hori);
            ReadSbitLineMetrics(reader, ref bmpSizeTable.vert);

            bmpSizeTable.startGlyphIndex = reader.ReadUInt16();
            bmpSizeTable.endGlyphIndex = reader.ReadUInt16();
            bmpSizeTable.ppemX = reader.ReadByte();
            bmpSizeTable.ppemY = reader.ReadByte();
            bmpSizeTable.bitDepth = reader.ReadByte();
            bmpSizeTable.flags = (sbyte)reader.ReadByte();

            return bmpSizeTable;
        }
    }
}
