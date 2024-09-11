using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format1 : IGlyphBitmapData
    {
        public SmallGlyphMetricsRecord SmallGlyphMetrics { get; }

        public byte[] BitmapData { get; }

        public Format1(BigEndianReader reader, uint dataSize)
        {
            SmallGlyphMetrics = new SmallGlyphMetricsRecord(reader);
            BitmapData = reader.ReadBytes(dataSize);
        }
    }
}