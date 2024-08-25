using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class LigCaretListTable
    {
        public ushort CoverageOffset { get; }

        public ushort LigGlyphCount { get; }

        public List<LigGlyphTable> LigGlyphOffsets { get; } = new List<LigGlyphTable>();

        public LigCaretListTable(BigEndianReader reader)
        {
            CoverageOffset = reader.ReadUShort();
            LigGlyphCount = reader.ReadUShort();
            for (var i = 0; i < LigGlyphCount; i++)
            {
                byte[] ligGlyphTableLengthData = reader.PeekBytes(2);
                var ligGlyphTableLength = (ushort)(ligGlyphTableLengthData[0] << 8 | ligGlyphTableLengthData[1]);
                LigGlyphOffsets.Add(new LigGlyphTable(reader.ReadBytes(ligGlyphTableLength)));
            }
        }
    }
}
