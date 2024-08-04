namespace FontParser.Typeface
{
    public class GlyphNameMap
    {
        public readonly ushort glyphIndex;
        public readonly string glyphName;

        public GlyphNameMap(ushort glyphIndex, string glyphName)
        {
            this.glyphIndex = glyphIndex;
            this.glyphName = glyphName;
        }
    }
}