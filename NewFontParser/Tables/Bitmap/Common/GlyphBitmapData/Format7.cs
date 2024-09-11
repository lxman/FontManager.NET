using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format7 : IGlyphBitmapData
    {
        public BigGlyphMetricsRecord BigGlyphMetrics { get; }

        public byte[] BitmapData { get; }

        public Format7(BigEndianReader reader, uint dataSize)
        {
            BigGlyphMetrics = new BigGlyphMetricsRecord(reader);
            BitmapData = reader.ReadBytes(dataSize);
        }
    }
}