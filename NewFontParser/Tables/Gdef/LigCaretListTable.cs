using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gdef
{
    public class LigCaretListTable
    {
        public ICoverageFormat Coverage { get; }

        public List<LigGlyphTable> LigGlyphOffsets { get; } = new List<LigGlyphTable>();

        public LigCaretListTable(BigEndianReader reader)
        {
            long startOfTable = reader.Position;

            ushort coverageOffset = reader.ReadUShort();
            ushort ligGlyphCount = reader.ReadUShort();
            ushort[] ligGlyphOffsets = reader.ReadUShortArray(ligGlyphCount);
            for (var i = 0; i < ligGlyphCount; i++)
            {
                if (ligGlyphOffsets[i] == 0) continue;
                reader.Seek(startOfTable + ligGlyphOffsets[i]);
                LigGlyphOffsets.Add(new LigGlyphTable(reader));
            }
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}