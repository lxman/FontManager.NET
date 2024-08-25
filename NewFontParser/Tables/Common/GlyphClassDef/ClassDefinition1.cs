using NewFontParser.Reader;
using NewFontParser.Tables.Gdef;

namespace NewFontParser.Tables.Common.GlyphClassDef
{
    public class ClassDefinition1 : IClassDefinition
    {
        public long Length { get; }

        public ushort Format => 1;

        public ushort StartGlyph { get; }

        public ushort GlyphCount { get; }

        public ushort[] Classes { get; }

        public ClassDefinition1(BigEndianReader reader)
        {
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
