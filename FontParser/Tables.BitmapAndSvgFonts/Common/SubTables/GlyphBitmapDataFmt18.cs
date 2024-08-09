using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// Format 18: big metrics, PNG image data
    /// </summary>
    public class GlyphBitmapDataFmt18 : GlyphBitmapDataFormatBase
    {
        //Format 18: big metrics, PNG image data
        //Type              Name            Description
        //bigGlyphMetrics   glyphMetrics    Metrics information for the glyph
        //uint32            dataLen         Length of data in bytes
        //uint8             data[dataLen]   Raw PNG data
        public override int FormatNumber => 18;

        public override void FillGlyphInfo(BinaryReader reader, Glyph bitmapGlyph)
        {
            var bigGlyphMetric = new BigGlyphMetrics();
            BigGlyphMetrics.ReadBigGlyphMetric(reader, ref bigGlyphMetric);
            uint dataLen = reader.ReadUInt32();

            bitmapGlyph.BitmapGlyphAdvanceWidth = bigGlyphMetric.horiAdvance;
            bitmapGlyph.Bounds = new Bounds(0, 0, bigGlyphMetric.width, bigGlyphMetric.height);
        }

        public override void ReadRawBitmap(BinaryReader reader, Glyph bitmapGlyph, Stream outputStream)
        {
            reader.BaseStream.Position += BigGlyphMetrics.SIZE;
            uint dataLen = reader.ReadUInt32();
            byte[] rawPngData = reader.ReadBytes((int)dataLen);
            outputStream.Write(rawPngData, 0, rawPngData.Length);
        }
    }
}
