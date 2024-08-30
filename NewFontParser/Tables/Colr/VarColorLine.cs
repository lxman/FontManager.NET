using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr
{
    public class VarColorLine
    {
        public ExtendMode ExtendMode { get; }

        public List<VarColorStop> ColorStops { get; } = new List<VarColorStop>();

        public VarColorLine(BigEndianReader reader)
        {
            ExtendMode = (ExtendMode)reader.ReadByte();
            ushort colorStopCount = reader.ReadUShort();
            for (var i = 0; i < colorStopCount; i++)
            {
                ColorStops.Add(new VarColorStop(reader));
            }
        }
    }
}
