using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format5 : IGlyphBitmapData
    {
        public byte[] Data { get; set; }

        public Format5(BigEndianReader reader)
        {
            // TODO: Figure out how to read the data
            Data = reader.ReadBytes(0);
        }
    }
}