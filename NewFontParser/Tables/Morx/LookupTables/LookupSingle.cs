using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class LookupSingle
    {
        public ushort GlyphIndex { get; }

        public byte[] Value { get; }

        public LookupSingle(BigEndianReader reader, ushort width)
        {
            GlyphIndex = reader.ReadUShort();
            Value = reader.ReadBytes(width - 2);
        }
    }
}
