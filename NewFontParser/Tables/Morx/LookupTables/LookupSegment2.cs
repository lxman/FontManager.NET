using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class LookupSegment2
    {
        public ushort LastGlyph { get; }

        public ushort FirstGlyph { get; }

        public byte[] Value { get; }

        public LookupSegment2(BigEndianReader reader, ushort width)
        {
            FirstGlyph = reader.ReadUShort();
            LastGlyph = reader.ReadUShort();
            Value = reader.ReadBytes(width - 4);
        }
    }
}
