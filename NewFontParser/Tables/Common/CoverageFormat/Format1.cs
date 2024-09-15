using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.CoverageFormat
{
    public class Format1 : ICoverageFormat
    {
        public ushort Format => 1;

        public ushort[] GlyphArray { get; }

        public Format1(BigEndianReader reader)
        {
            _ = reader.ReadUShort(); // Skip format
            ushort glyphCount = reader.ReadUShort();
            GlyphArray = new ushort[glyphCount];
            for (var i = 0; i < glyphCount; i++)
            {
                GlyphArray[i] = reader.ReadUShort();
            }
        }
    }
}