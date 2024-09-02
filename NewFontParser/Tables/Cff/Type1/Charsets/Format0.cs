using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1.Charsets
{
    public class Format0 : ICharset
    {
        public ushort[] Glyphs { get; }

        public Format0(BigEndianReader reader, ushort numGlyphs)
        {
            Glyphs = new ushort[numGlyphs];
            for (var i = 0; i < numGlyphs - 1; i++)
            {
                Glyphs[i] = reader.ReadUShort();
            }
        }
    }
}
