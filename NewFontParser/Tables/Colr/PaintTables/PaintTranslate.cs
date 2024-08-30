using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintTranslate : IPaintTable
    {
        public byte Format => 14;

        public IPaintTable SubTable { get; }

        public short Dx { get; }

        public short Dy { get; }

        public PaintTranslate(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            Dx = reader.ReadShort();
            Dy = reader.ReadShort();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}
