using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common;

namespace NewFontParser.Tables.Proprietary.Aat.Bloc.BitmapIndexSubtable
{
    public class Format2
    {
        public IndexFormat IndexFormat { get; }

        public ImageFormat ImageFormat { get; }

        public BigGlyphMetricsRecord BigGlyphMetrics { get; }

        public Format2(BigEndianReader reader)
        {
            IndexFormat = (IndexFormat)reader.ReadUShort();
            ImageFormat = (ImageFormat)reader.ReadUShort();
            uint imageDataOffset = reader.ReadUInt32();
            uint imageSize = reader.ReadUInt32();
            BigGlyphMetrics = new BigGlyphMetricsRecord(reader);
        }
    }
}