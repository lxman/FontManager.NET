using System;
using System.Diagnostics;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Math
{
    public class MathValueRecord
    {
        public short Value { get; }

        public DeviceTable? DeviceTable { get; }

        public MathValueRecord(BigEndianReader reader, long parentTableOrigin)
        {
            Value = reader.ReadShort();
            ushort deviceOffset = reader.ReadUShort();
            if (deviceOffset == 0)
            {
                return;
            }
            long before = reader.Position;
            reader.Seek(parentTableOrigin + deviceOffset);
            DeviceTable = new DeviceTable(reader);
            reader.Seek(before);
        }
    }
}