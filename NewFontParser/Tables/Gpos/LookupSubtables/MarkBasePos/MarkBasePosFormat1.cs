using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Gpos.LookupSubtables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkBasePos
{
    public class MarkBasePosFormat1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat MarkCoverage { get; }

        public ICoverageFormat BaseCoverage { get; }

        public MarkArray MarkArray { get; }

        public BaseArrayTable BaseArray { get; }

        public MarkBasePosFormat1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort markCoverageOffset = reader.ReadUShort();
            ushort baseCoverageOffset = reader.ReadUShort();
            ushort markClassCount = reader.ReadUShort();
            ushort markArrayOffset = reader.ReadUShort();
            ushort baseArrayOffset = reader.ReadUShort();
            reader.Seek(markArrayOffset + startOfTable);
            MarkArray = new MarkArray(reader);
            reader.Seek(baseArrayOffset + startOfTable);
            BaseArray = new BaseArrayTable(reader, markClassCount);
            reader.Seek(startOfTable + markCoverageOffset);
            MarkCoverage = CoverageTable.Retrieve(reader);
            reader.Seek(startOfTable + baseCoverageOffset);
            BaseCoverage = CoverageTable.Retrieve(reader);
        }
    }
}