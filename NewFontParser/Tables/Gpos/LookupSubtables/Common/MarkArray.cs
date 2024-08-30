using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.Common
{
    public class MarkArray
    {
        public ushort MarkCount { get; }

        public MarkRecord[] MarkRecords { get; }

        public MarkArray(BigEndianReader reader)
        {
            MarkCount = reader.ReadUShort();
            MarkRecords = new MarkRecord[MarkCount];
            for (var i = 0; i < MarkCount; i++)
            {
                MarkRecords[i] = new MarkRecord(reader.ReadBytes(4));
            }
        }
    }
}
