using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common;

namespace NewFontParser.Tables.Proprietary.Aat.Bdat.GlyphBitmap
{
    public class Format2 : IGlyphBitmap
    {
        public SmallGlyphMetricsRecord SmallGlyphMetrics { get; }

        public byte[] ImageData { get; }

        public Format2(BigEndianReader reader)
        {
            SmallGlyphMetrics = new SmallGlyphMetricsRecord(reader);
            ImageData = reader.ReadBytes(reader.BytesRemaining);
        }
    }
}