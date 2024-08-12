namespace NewFontParser.Tables.TtTables.Glyf
{
    public class GlyphData
    {
        public GlyphHeader Header { get; }

        public IGlyphSpec GlyphSpec { get; }

        public GlyphData(GlyphHeader header, IGlyphSpec glyphSpec)
        {
            Header = header;
            GlyphSpec = glyphSpec;
        }
    }
}
