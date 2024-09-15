using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos
{
    public class SinglePosFormat2Table : ISinglePosFormatTable
    {
        public ushort PosFormat { get; }

        public ushort CoverageOffset { get; }

        public ValueFormat ValueFormat { get; }

        public ushort ValueCount { get; }

        public ValueRecord[] ValueRecords { get; }

        public SinglePosFormat2Table(byte[] data)
        {
            var reader = new BigEndianReader(data);

            PosFormat = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ValueFormat = (ValueFormat)reader.ReadUShort();
            ValueCount = reader.ReadUShort();

            ValueRecords = new ValueRecord[ValueCount];
            for (var i = 0; i < ValueCount; i++)
            {
                ValueRecords[i] = new ValueRecord(ValueFormat, reader);
            }
        }
    }
}