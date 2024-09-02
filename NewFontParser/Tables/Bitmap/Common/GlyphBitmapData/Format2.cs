using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format2 : IGlyphBitmapDataFormat
    {
        public SmallGlyphMetricsRecord SmallGlyphMetrics { get; }

        public byte[] BitmapData { get; }

        public Format2(BigEndianReader reader)
        {
            SmallGlyphMetrics = new SmallGlyphMetricsRecord(reader);
            // TODO: Figure out how to read the bitmap data
            BitmapData = reader.ReadBytes(SmallGlyphMetrics.Width * SmallGlyphMetrics.Height);
        }
    }
}
