using NewFontParser.Reader;

namespace NewFontParser.Tables.Math
{
    public class GlyphPartRecord
    {
        public ushort GlyphId { get; }

        public ushort StartConnectorLength { get; }

        public ushort EndConnectorLength { get; }

        public ushort FullAdvance { get; }

        public PartFlags PartFlags { get; }

        public GlyphPartRecord(BigEndianReader reader)
        {
            GlyphId = reader.ReadUShort();
            StartConnectorLength = reader.ReadUShort();
            EndConnectorLength = reader.ReadUShort();
            FullAdvance = reader.ReadUShort();
            PartFlags = (PartFlags)reader.ReadByte();
        }
    }
}
