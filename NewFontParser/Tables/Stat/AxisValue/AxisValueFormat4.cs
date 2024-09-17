using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Stat.AxisValue
{
    public class AxisValueFormat4 : IAxisValueTable
    {
        public ushort Format { get; }

        public ushort AxisCount { get; }

        public AxisValueFlags Flags { get; }

        public ushort ValueNameId { get; }

        public List<AxisValueRecord> AxisValues { get; } = new List<AxisValueRecord>();

        public AxisValueFormat4(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            AxisCount = reader.ReadUShort();
            Flags = (AxisValueFlags)reader.ReadUShort();
            ValueNameId = reader.ReadUShort();

            for (var i = 0; i < AxisCount; i++)
            {
                AxisValues.Add(new AxisValueRecord(reader));
            }
        }
    }
}