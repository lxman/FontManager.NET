using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Fvar
{
    public class VariationAxisRecord
    {
        public string AxisTag { get; }

        public float MinValue { get; }

        public float DefaultValue { get; }

        public float MaxValue { get; }

        public AxisFlags Flags { get; }

        public ushort AxisNameId { get; }

        public VariationAxisRecord(BigEndianReader reader)
        {
            AxisTag = Encoding.ASCII.GetString(reader.ReadBytes(4));
            MinValue = reader.ReadF16Dot16();
            DefaultValue = reader.ReadF16Dot16();
            MaxValue = reader.ReadF16Dot16();
            Flags = (AxisFlags)reader.ReadUShort();
            AxisNameId = reader.ReadUShort();
        }
    }
}
