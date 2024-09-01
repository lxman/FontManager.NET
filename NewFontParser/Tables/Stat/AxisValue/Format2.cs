﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Stat.AxisValue
{
    public class Format2 : IAxisValueTable
    {
        public ushort Format { get; }

        public ushort AxisIndex { get; }

        public AxisValueFlags Flags { get; }

        public ushort ValueNameId { get; }

        public float NominalValue { get; }

        public float RangeMinValue { get; }

        public float RangeMaxValue { get; }

        public Format2(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            AxisIndex = reader.ReadUShort();
            Flags = (AxisValueFlags)reader.ReadUShort();
            ValueNameId = reader.ReadUShort();
            NominalValue = reader.ReadF16Dot16();
            RangeMinValue = reader.ReadF16Dot16();
            RangeMaxValue = reader.ReadF16Dot16();
        }
    }
}
