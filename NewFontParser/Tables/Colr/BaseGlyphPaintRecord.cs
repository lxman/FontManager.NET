using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr
{
    public class BaseGlyphPaintRecord
    {
        public ushort GlyphId { get; }

        public IPaintTable SubTable { get; }

        public BaseGlyphPaintRecord(BigEndianReader reader)
        {
            GlyphId = reader.ReadUShort();
            uint offset = reader.ReadUInt32();
            SubTable = PaintTableFactory.CreatePaintTable(reader, offset);
        }
    }
}
