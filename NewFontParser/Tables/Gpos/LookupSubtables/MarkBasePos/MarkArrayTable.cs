using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkBasePos
{
    public class MarkArrayTable
    {
        public ushort MarkCount { get; }

        public List<MarkRecord> MarkRecords { get; } = new List<MarkRecord>();

        public MarkArrayTable(BigEndianReader reader)
        {
            MarkCount = reader.ReadUShort();
            for (var i = 0; i < MarkCount; i++)
            {
                MarkRecords.Add(new MarkRecord(reader.ReadBytes(4)));
            }
        }
    }
}
