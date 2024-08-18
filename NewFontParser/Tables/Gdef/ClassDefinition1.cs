using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class ClassDefinition1 : IClassDefinition
    {
        public long Length { get; }

        public ushort Format { get; } = 1;

        public ushort StartGlyph { get; }

        public ushort GlyphCount { get; }

        public ushort[] Classes { get; }

        public ClassDefinition1(byte[] data)
        {
            var reader = new BigEndianReader(data);
            _ = reader.ReadBytes(2);
            StartGlyph = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            Length = 6;
            Classes = new ushort[GlyphCount];
            for (var i = 0; i < GlyphCount; i++)
            {
                Classes[i] = reader.ReadUShort();
                Length += 2;
            }
        }
    }
}
