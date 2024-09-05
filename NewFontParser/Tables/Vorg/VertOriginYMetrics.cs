using NewFontParser.Reader;

namespace NewFontParser.Tables.Vorg
{
    public class VertOriginYMetrics
    {
        public ushort GlyphIndex { get; }

        public short VertOriginY { get; }

        public VertOriginYMetrics(BigEndianReader reader)
        {
            GlyphIndex = reader.ReadUShort();
            VertOriginY = reader.ReadShort();
        }
    }
}
