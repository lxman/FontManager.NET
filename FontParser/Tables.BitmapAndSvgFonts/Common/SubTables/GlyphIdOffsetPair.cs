namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    //GlyphIdOffsetPair record:
    //Type      Name        Description
    //uint16    glyphID     Glyph ID of glyph present.
    //Offset16  offset      Location in EBDT.

    public class GlyphIdOffsetPair
    {
        public readonly ushort glyphId;
        public readonly ushort offset;

        public GlyphIdOffsetPair(ushort glyphId, ushort offset)
        {
            this.glyphId = glyphId;
            this.offset = offset;
        }
    }
}
