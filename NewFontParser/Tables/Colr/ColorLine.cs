using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr
{
    public class ColorLine
    {
        public ExtendMode ExtendMode { get; }

        public List<ColorStop> ColorStops { get; } = new List<ColorStop>();

        public ColorLine(BigEndianReader reader)
        {
            ExtendMode = (ExtendMode)reader.ReadByte();
            ushort colorStopCount = reader.ReadUShort();
            for (var i = 0; i < colorStopCount; i++)
            {
                ColorStops.Add(new ColorStop(reader));
            }
        }
    }
}