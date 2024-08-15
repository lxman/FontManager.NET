using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class LigGlyphTable
    {
        public ushort CaretCount { get; }

        public List<ushort> CaretValueOffsets { get; } = new List<ushort>();

        public LigGlyphTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            CaretCount = reader.ReadUShort();
            for (var i = 0; i < CaretCount; i++)
            {
                CaretValueOffsets.Add(reader.ReadUShort());
            }
        }
    }
}
