using NewFontParser.Reader;

namespace NewFontParser.Tables.CoverageFormat
{
    public class Format2 : ICoverageFormat
    {
        public ushort Format => 2;

        public ushort RangeCount { get; }

        public RangeRecord[] RangeRecords { get; }

        public Format2(BigEndianReader reader)
        {
            _ = reader.ReadUShort(); // Skip format
            RangeCount = reader.ReadUShort();
            RangeRecords = new RangeRecord[RangeCount];
            for (var i = 0; i < RangeCount; i++)
            {
                RangeRecords[i] = new RangeRecord(reader.ReadBytes(6));
            }
        }
    }
}
