using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gsub.LookupSubTables.SingleSubstitution
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat Coverage { get; }

        public ushort DeltaGlyphId { get; }

        public Format1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            DeltaGlyphId = reader.ReadUShort();
        }
    }
}
