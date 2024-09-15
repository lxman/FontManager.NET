using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.CoverageFormat
{
    public class Format2 : ICoverageFormat
    {
        public ushort Format => 2;

        public RangeRecord[] RangeRecords { get; }

        public Format2(BigEndianReader reader)
        {
            _ = reader.ReadUShort(); // Skip format
            ushort rangeCount = reader.ReadUShort();
            RangeRecords = new RangeRecord[rangeCount];
            for (var i = 0; i < rangeCount; i++)
            {
                RangeRecords[i] = new RangeRecord(reader.ReadBytes(6));
            }
        }
    }
}