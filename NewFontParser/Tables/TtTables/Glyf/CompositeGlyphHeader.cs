using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class CompositeGlyphHeader
    {
        public short Flags { get; }

        public short GlyphIndex { get; }

        public short Argument1 { get; }

        public short Argument2 { get; }

        public CompositeGlyphHeader(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Flags = reader.ReadShort();
            GlyphIndex = reader.ReadShort();
            Argument1 = reader.ReadShort();
            Argument2 = reader.ReadShort();
        }
    }
}
