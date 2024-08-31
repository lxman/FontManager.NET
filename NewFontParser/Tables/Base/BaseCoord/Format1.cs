using NewFontParser.Reader;

namespace NewFontParser.Tables.Base.BaseCoord
{
    public class Format1 : IBaseCoordFormat
    {
        public ushort BaseCoordFormat => 1;

        public short Coordinate { get; }

        public Format1(BigEndianReader reader)
        {
            Coordinate = reader.ReadShort();
        }
    }
}
