using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class AttachListTable
    {
        public ushort CoverageOffset { get; }

        public ushort GlyphCount { get; }

        public List<ushort> AttachPointOffsets { get; } = new List<ushort>();

        public AttachListTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            CoverageOffset = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            for (var i = 0; i < GlyphCount; i++)
            {
                AttachPointOffsets.Add(reader.ReadUShort());
            }
        }
    }
}
