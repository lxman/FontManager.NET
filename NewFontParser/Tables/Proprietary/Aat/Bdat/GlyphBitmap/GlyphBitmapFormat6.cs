using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common;

namespace NewFontParser.Tables.Proprietary.Aat.Bdat.GlyphBitmap
{
    public class GlyphBitmapFormat6 : IGlyphBitmap
    {
        public BigGlyphMetricsRecord BigGlyphMetrics { get; }

        public byte[] ImageData { get; }

        public GlyphBitmapFormat6(BigEndianReader reader)
        {
            BigGlyphMetrics = new BigGlyphMetricsRecord(reader);
            ImageData = reader.ReadBytes(reader.BytesRemaining);
        }
    }
}