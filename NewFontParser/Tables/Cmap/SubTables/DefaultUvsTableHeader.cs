using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class DefaultUvsTableHeader
    {
        public uint NumUnicodeRangeRecords { get; }

        public List<UnicodeRangeRecord> UnicodeRangeRecords { get; } = new List<UnicodeRangeRecord>();

        public DefaultUvsTableHeader(BigEndianReader reader)
        {
            NumUnicodeRangeRecords = reader.ReadUint32();
            for (var i = 0; i < NumUnicodeRangeRecords; i++)
            {
                UnicodeRangeRecords.Add(new UnicodeRangeRecord(reader.ReadBytes(UnicodeRangeRecord.RecordSize)));
            }
        }
    }
}