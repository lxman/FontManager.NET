using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    ///  Format 17: small metrics, PNG image data
    /// </summary>
    public class GlyphBitmapDataFmt17 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 17;

        //Format 17: small metrics, PNG image data
        //Type                Name          Description
        //smallGlyphMetrics   glyphMetrics  Metrics information for the glyph
        //uint32              dataLen       Length of data in bytes
        //uint8               data[dataLen] Raw PNG data

        public override void FillGlyphInfo(BinaryReader reader, Glyph bitmapGlyph)
        {
            SmallGlyphMetrics.ReadSmallGlyphMetric(reader, out SmallGlyphMetrics smallGlyphMetric);

            bitmapGlyph.BitmapGlyphAdvanceWidth = smallGlyphMetric.advance;
            bitmapGlyph.Bounds = new Bounds(0, 0, smallGlyphMetric.width, smallGlyphMetric.height);

            //then
            //byte[] buff = reader.ReadBytes((int)dataLen);
            //System.IO.File.WriteAllBytes("testBitmapGlyph_" + glyph.GlyphIndex + ".png", buff);
        }

        public override void ReadRawBitmap(BinaryReader reader, Glyph bitmapGlyph, Stream outputStream)
        {
            //only read raw png data
            reader.BaseStream.Position += SmallGlyphMetrics.SIZE;
            uint dataLen = reader.ReadUInt32();
            byte[] rawPngData = reader.ReadBytes((int)dataLen);
            outputStream.Write(rawPngData, 0, rawPngData.Length);
        }
    }
}
