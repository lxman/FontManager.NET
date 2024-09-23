using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gsub.LookupSubTables.SingleSubstitution
{
    public class SingleSubstitutionFormat2 : ILookupSubTable
    {
        public ICoverageFormat Coverage { get; }

        public ushort[] SubstituteGlyphIds { get; }

        public SingleSubstitutionFormat2(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            _ = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort glyphCount = reader.ReadUShort();
            SubstituteGlyphIds = reader.ReadUShortArray(glyphCount);
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}