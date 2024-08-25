using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkBasePos
{
    public class BaseArrayTable
    {
        public ushort BaseCount { get; }

        public BaseRecord[] BaseRecords { get; }

        public BaseArrayTable(BigEndianReader reader, ushort markClassCount)
        {
            BaseCount = reader.ReadUShort();
            BaseRecords = new BaseRecord[BaseCount];
            for (var i = 0; i < BaseCount; i++)
            {
                BaseRecords[i] = new BaseRecord(markClassCount, reader);
            }
        }
    }
}
