using System.Drawing;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Pfed.SubTables
{
    public class ColrItem
    {
        public ushort StartingGlyphId { get; }

        public ushort EndingGlyphId { get; }

        public Color Color { get; }

        public ColrItem(BigEndianReader reader)
        {
            StartingGlyphId = reader.ReadUShort();
            EndingGlyphId = reader.ReadUShort();
            _ = reader.ReadByte();
            Color = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }
    }
}