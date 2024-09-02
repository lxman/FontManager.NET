using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format7 : IGlyphBitmapDataFormat
    {
        public BigGlyphMetricsRecord BigGlyphMetrics { get; }

        public byte[] BitmapData { get; }

        public Format7(BigEndianReader reader)
        {
            BigGlyphMetrics = new BigGlyphMetricsRecord(reader);
            // TODO: Figure out how to read the bitmap data
            BitmapData = reader.ReadBytes(BigGlyphMetrics.Width * BigGlyphMetrics.Height);
        }
    }
}
