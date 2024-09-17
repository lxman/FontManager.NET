using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Morx.LookupTables
{
    public class LookupTablesFormat8 : IFsHeader
    {
        public ushort FirstGlyph { get; }

        public ushort GlyphCount { get; }

        public List<byte[]> Values { get; } = new List<byte[]>();

        public LookupTablesFormat8(BigEndianReader reader)
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