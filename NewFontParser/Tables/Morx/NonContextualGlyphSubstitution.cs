using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx
{
    public class NonContextualGlyphSubstitution
    {
        public LookupTable LookupTable { get; }

        public NonContextualGlyphSubstitution(BigEndianReader reader)
        {
            LookupTable = new LookupTable(reader);
        }
    }
}
