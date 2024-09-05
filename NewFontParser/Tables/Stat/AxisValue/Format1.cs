using NewFontParser.Reader;

namespace NewFontParser.Tables.Stat.AxisValue
{
    public class Format1 : IAxisValueTable
    {
        public ushort Format { get; }

        public ushort AxisIndex { get; }

        public AxisValueFlags Flags { get; }

        public ushort ValueNameId { get; }

        public float Value { get; }

        public Format1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            AxisIndex = reader.ReadUShort();
            Flags = (AxisValueFlags)reader.ReadUShort();
            ValueNameId = reader.ReadUShort();
            Value = reader.ReadF16Dot16();
        }
    }
}