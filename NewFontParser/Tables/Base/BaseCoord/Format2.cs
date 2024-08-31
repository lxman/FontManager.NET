using NewFontParser.Reader;

namespace NewFontParser.Tables.Base.BaseCoord
{
    public class Format2 : IBaseCoordFormat
    {
        public ushort BaseCoordFormat => 2;

        public short Coordinate { get; }

        public ushort ReferenceGlyph { get; }

        public ushort BaseCoordPoint { get; }

        public Format2(BigEndianReader reader)
        {
            Coordinate = reader.ReadShort();
            ReferenceGlyph = reader.ReadUShort();
            BaseCoordPoint = reader.ReadUShort();
        }
    }
}
