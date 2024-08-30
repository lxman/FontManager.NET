using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintScale : IPaintTable
    {
        public byte Format => 16;

        public IPaintTable SubTable { get; }

        public float ScaleX { get; }

        public float ScaleY { get; }

        public PaintScale(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            ScaleX = reader.ReadF2Dot14();
            ScaleY = reader.ReadF2Dot14();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}
