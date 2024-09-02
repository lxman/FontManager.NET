using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format6 : IGlyphBitmapDataFormat
    {
        public BigGlyphMetricsRecord BigMetrics { get; }

        public byte[] BitmapData { get; }

        public Format6(BigEndianReader reader)
        {
            BigMetrics = new BigGlyphMetricsRecord(reader);
            BitmapData = reader.ReadBytes(BigMetrics.Height * BigMetrics.Width);
        }
    }
}
