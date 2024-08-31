using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class Format10 : IFsHeader
    {
        public ushort UnitSize { get; }

        public ushort FirstGlyph { get; }

        public ushort GlyphCount { get; }

        public List<byte[]> Values { get; } = new List<byte[]>();

        public Format10(BigEndianReader reader)
        {
            UnitSize = reader.ReadUShort();
            FirstGlyph = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            for (var i = 0; i < GlyphCount; i++)
            {
                Values.Add(reader.ReadBytes(UnitSize));
            }
        }
    }
}
