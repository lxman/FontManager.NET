using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintScaleUniformAroundCenter : IPaintTable
    {
        public byte Format => 22;

        public IPaintTable SubTable { get; }

        public float Scale { get; }

        public short CenterX { get; }

        public short CenterY { get; }

        public PaintScaleUniformAroundCenter(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            Scale = reader.ReadF2Dot14();
            CenterX = reader.ReadShort();
            CenterY = reader.ReadShort();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}
