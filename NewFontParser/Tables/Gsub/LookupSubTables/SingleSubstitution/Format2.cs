using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gsub.LookupSubTables.SingleSubstitution
{
    public class Format2 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort[] SubstituteGlyphIds { get; }

        public Format2(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort glyphCount = reader.ReadUShort();
            SubstituteGlyphIds = reader.ReadUShortArray(glyphCount);
        }
    }
}
