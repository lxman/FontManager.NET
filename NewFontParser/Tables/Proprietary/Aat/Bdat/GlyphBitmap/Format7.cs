using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common;

namespace NewFontParser.Tables.Proprietary.Aat.Bdat.GlyphBitmap
{
    public class Format7 : IGlyphBitmap
    {
        public BigGlyphMetricsRecord BigGlyphMetrics { get; }

        public byte[] BitmapData { get; }

        public Format7(BigEndianReader reader)
        {
            BigGlyphMetrics = new BigGlyphMetricsRecord(reader);
            BitmapData = reader.ReadBytes(reader.BytesRemaining);
        }
    }
}