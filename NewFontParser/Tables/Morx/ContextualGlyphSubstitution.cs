using NewFontParser.Reader;
using NewFontParser.Tables.Morx.StateTables;

namespace NewFontParser.Tables.Morx
{
    public class ContextualGlyphSubstitution
    {
        public StxHeader Header { get; }

        public uint SubstitutionTable { get; }

        public ContextualGlyphSubstitution(BigEndianReader reader)
        {
            Header = new StxHeader(reader);
            SubstitutionTable = reader.ReadUInt32();
        }
    }
}
