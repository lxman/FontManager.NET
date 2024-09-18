using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1.Charsets
{
    public class CharsetsFormat0 : ICharset
    {
        public ushort[] Glyphs { get; }

        public CharsetsFormat0(BigEndianReader reader, ushort numGlyphs)
        {
            Glyphs = new ushort[numGlyphs];
            for (var i = 0; i < numGlyphs - 1; i++)
            {
                Glyphs[i] = reader.ReadUShort();
            }
        }
    }
}