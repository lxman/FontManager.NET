using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class LigGlyphTable
    {
        public List<ushort> CaretValueOffsets { get; } = new List<ushort>();

        public LigGlyphTable(BigEndianReader reader)
        {
            ushort caretCount = reader.ReadUShort();
            for (var i = 0; i < caretCount; i++)
            {
                CaretValueOffsets.Add(reader.ReadUShort());
            }
        }
    }
}