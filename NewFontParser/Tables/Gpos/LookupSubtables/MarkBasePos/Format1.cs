using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkBasePos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort MarkCoverageOffset { get; }

        public ushort BaseCoverageOffset { get; }

        public ushort MarkClassCount { get; }

        public MarkArrayTable MarkArray { get; }

        public BaseArrayTable BaseArray { get; }

        public Format1(BigEndianReader reader)
        {
            long position = reader.Position;
            Format = reader.ReadUShort();
            MarkCoverageOffset = reader.ReadUShort();
            BaseCoverageOffset = reader.ReadUShort();
            MarkClassCount = reader.ReadUShort();
            ushort markArrayOffset = reader.ReadUShort();
            ushort baseArrayOffset = reader.ReadUShort();
            reader.Seek(markArrayOffset + position);
            MarkArray = new MarkArrayTable(reader);
            reader.Seek(baseArrayOffset + position);
            BaseArray = new BaseArrayTable(reader, MarkClassCount);
        }
    }
}
