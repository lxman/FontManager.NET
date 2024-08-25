using System;
using System.Collections.Generic;
using System.Linq;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class ValueRecord
    {
        public short? XPlacement { get; internal set; }

        public short? YPlacement { get; internal set; }

        public short? XAdvance { get; internal set; }

        public short? YAdvance { get; internal set; }

        public ushort? XPlaDeviceOffset { get; internal set; }

        public ushort? YPlaDeviceLength { get; internal set; }

        public ushort? XAdvDeviceOffset { get; internal set; }

        public ushort? YAdvDeviceLength { get; internal set; }

        public ValueRecord(IEnumerable<ValueFormat> flags, BigEndianReader reader)
        {
            flags.ToList().ForEach(f =>
            {
                switch (f)
                {
                    case ValueFormat.XPlacement:
                        XPlacement = reader.ReadShort();
                        break;
                    case ValueFormat.YPlacement:
                        YPlacement = reader.ReadShort();
                        break;
                    case ValueFormat.XAdvance:
                        XAdvance = reader.ReadShort();
                        break;
                    case ValueFormat.YAdvance:
                        YAdvance = reader.ReadShort();
                        break;
                    case ValueFormat.XPlacementDevice:
                        XPlaDeviceOffset = reader.ReadUShort();
                        break;
                    case ValueFormat.YPlacementDevice:
                        YPlaDeviceLength = reader.ReadUShort();
                        break;
                    case ValueFormat.XAdvanceDevice:
                        XAdvDeviceOffset = reader.ReadUShort();
                        break;
                    case ValueFormat.YAdvanceDevice:
                        YAdvDeviceLength = reader.ReadUShort();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(f), f, null);
                }
            });
        }
    }
}
