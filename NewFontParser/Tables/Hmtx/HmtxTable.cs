using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Hmtx
{
    public class HmtxTable : IFontTable
    {
        public static string Tag => "hmtx";

        public List<LongHMetricRecord> LongHMetricRecords { get; } = new List<LongHMetricRecord>();

        public List<short> LeftSideBearings { get; } = new List<short>();

        private readonly BigEndianReader _reader;

        public HmtxTable(byte[] data)
        {
            _reader = new BigEndianReader(data);
        }

        // numberOfHMetricRecords: From the 'hhea' table.
        // numOfGlyphs: From the 'maxp' table.
        public void Process(ushort numberOfHMetricRecords, ushort numOfGlyphs)
        {
            for (var i = 0; i < numberOfHMetricRecords; i++)
            {
                LongHMetricRecords.Add(new LongHMetricRecord(_reader.ReadBytes(LongHMetricRecord.RecordSize)));
            }

            if (LongHMetricRecords.Count >= numOfGlyphs) return;
            {
                for (int i = LongHMetricRecords.Count; i < numOfGlyphs; i++)
                {
                    LeftSideBearings.Add(_reader.ReadShort());
                }
            }
        }
    }
}