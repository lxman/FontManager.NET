﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Stat.AxisValue
{
    public class AxisValueRecord
    {
        public ushort AxisIndex { get; }

        public float Value { get; }

        public AxisValueRecord(BigEndianReader reader)
        {
            AxisIndex = reader.ReadUShort();
            Value = reader.ReadF16Dot16();
        }
    }
}