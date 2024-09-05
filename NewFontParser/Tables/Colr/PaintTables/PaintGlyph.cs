using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintGlyph : IPaintTable
    {
        public byte Format => 10;

        public uint PaintOffset { get; }

        public ushort GlyphId { get; }

        public PaintGlyph(BigEndianReader reader)
        {
            PaintOffset = reader.ReadUInt24();
            GlyphId = reader.ReadUShort();
        }
    }
}