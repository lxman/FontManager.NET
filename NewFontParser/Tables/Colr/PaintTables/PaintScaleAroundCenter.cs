using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintScaleAroundCenter : IPaintTable
    {
        public byte Format => 18;

        public IPaintTable SubTable { get; }

        public float ScaleX { get; }

        public float ScaleY { get; }

        public short CenterX { get; }

        public short CenterY { get; }

        public PaintScaleAroundCenter(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            ScaleX = reader.ReadF16Dot16();
            ScaleY = reader.ReadF16Dot16();
            CenterX = reader.ReadShort();
            CenterY = reader.ReadShort();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}