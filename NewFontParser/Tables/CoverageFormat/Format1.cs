using NewFontParser.Reader;

namespace NewFontParser.Tables.CoverageFormat
{
    public class Format1 : ICoverageFormat
    {
        public ushort Format => 1;

        public ushort GlyphCount { get; }

        public ushort[] GlyphArray { get; }

        public Format1(BigEndianReader reader)
        {
            _ = reader.ReadUShort(); // Skip format
            GlyphCount = reader.ReadUShort();
            GlyphArray = new ushort[GlyphCount];
            for (var i = 0; i < GlyphCount; i++)
            {
                GlyphArray[i] = reader.ReadUShort();
            }
        }
    }
}