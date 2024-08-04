using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    public abstract class GlyphBitmapDataFormatBase
    {
        public abstract int FormatNumber { get; }

        public abstract void FillGlyphInfo(BinaryReader reader, Glyph bitmapGlyph);

        public abstract void ReadRawBitmap(BinaryReader reader, Glyph bitmapGlyph, Stream outputStream);
    }
}
