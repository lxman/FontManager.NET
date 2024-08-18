using NewFontParser.Reader;

namespace NewFontParser.Tables.CoverageFormat
{
    public class RangeRecord
    {
        public ushort Start { get; }
        public ushort End { get; }
        public ushort StartCoverageIndex { get; }

        public RangeRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Start = reader.ReadUShort();
            End = reader.ReadUShort();
            StartCoverageIndex = reader.ReadUShort();
        }
    }
}
