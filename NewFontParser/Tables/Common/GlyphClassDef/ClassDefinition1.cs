using NewFontParser.Reader;
using NewFontParser.Tables.Gdef;

namespace NewFontParser.Tables.Common.GlyphClassDef
{
    public class ClassDefinition1 : IClassDefinition
    {
        public ushort Format => 1;

        public ushort StartGlyph { get; }

        public ushort[] Classes { get; }

        public ClassDefinition1(BigEndianReader reader)
        {
            _ = reader.ReadBytes(2);
            StartGlyph = reader.ReadUShort();
            ushort glyphCount = reader.ReadUShort();
            Classes = reader.ReadUShortArray(glyphCount);
        }
    }
}