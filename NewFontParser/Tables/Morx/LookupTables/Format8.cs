using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class Format8 : IFsHeader
    {
        public ushort FirstGlyph { get; }

        public ushort GlyphCount { get; }

        public List<byte[]> Values { get; } = new List<byte[]>();

        public Format8(BigEndianReader reader)
        {
            FirstGlyph = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            for (var i = 0; i < GlyphCount; i++)
            {
                Values.Add(reader.ReadBytes(2));
            }
        }
    }
}
