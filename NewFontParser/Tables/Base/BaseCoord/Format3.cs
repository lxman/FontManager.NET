using NewFontParser.Reader;

namespace NewFontParser.Tables.Base.BaseCoord
{
    public class Format3 : IBaseCoordFormat
    {
        public ushort BaseCoordFormat => 3;

        public short Coordinate { get; }

        public ushort DeviceOffset { get; }

        public Format3(BigEndianReader reader)
        {
            Coordinate = reader.ReadShort();
            DeviceOffset = reader.ReadUShort();
        }
    }
}
