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
            long position = reader.Position;

            CoverageOffset = reader.ReadUShort();
            LigGlyphCount = reader.ReadUShort();
            ushort[] ligGlyphOffsets = reader.ReadUShortArray(LigGlyphCount);
            for (var i = 0; i < LigGlyphCount; i++)
            {
                if (ligGlyphOffsets[i] == 0) continue;
                reader.Seek(position + ligGlyphOffsets[i]);
                LigGlyphOffsets.Add(new LigGlyphTable(reader));
            }
        }
    }
}