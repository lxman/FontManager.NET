using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.Common
{
    public class MarkArray
    {
        public MarkRecord[] MarkRecords { get; }

        public MarkArray(BigEndianReader reader, ushort markCount)
        {
            MarkRecords = new MarkRecord[markCount];
            for (var i = 0; i < markCount; i++)
            {
                MarkRecords[i] = new MarkRecord(reader.ReadBytes(4));
            }
        }
    }
}
