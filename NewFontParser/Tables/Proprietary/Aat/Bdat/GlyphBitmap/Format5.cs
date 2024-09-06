using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Bdat.GlyphBitmap
{
    internal class Format5 : IGlyphBitmap
    {
        public byte[] ImageData { get; }

        public Format5(BigEndianReader reader)
        {
            ImageData = reader.ReadBytes(reader.BytesRemaining);
        }
    }
}