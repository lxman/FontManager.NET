using NewFontParser.Reader;
using NewFontParser.Tables.Proprietary.Aat.Morx.StateTables;

namespace NewFontParser.Tables.Proprietary.Aat.Morx
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