using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Hmtx
{
    public class HmtxTable : IInfoTable
    {
        public List<LongHMetricRecord> LongHMetricRecords { get; } = new List<LongHMetricRecord>();

        // numberOfHMetricRecords: From the 'hhea' table.
        // numOfGlyphs: From the 'maxp' table.
        public HmtxTable(byte[] data, ushort numberOfHMetricRecords, ushort numOfGlyphs)
        {
            var reader = new BigEndianReader(data);
            for (var i = 0; i < numberOfHMetricRecords; i++)
            {
                LongHMetricRecords.Add(new LongHMetricRecord(reader.ReadBytes(LongHMetricRecord.RecordSize)));
            }

            if (LongHMetricRecords.Count >= numOfGlyphs) return;
            {
                for (int i = LongHMetricRecords.Count; i < numOfGlyphs; i++)
                {
                    // Not quite sure whether this is the right way to handle this case.
                    LongHMetricRecords.Add(new LongHMetricRecord(reader.ReadBytes(LongHMetricRecord.RecordSize)));
                }
            }
        }
    }
}
