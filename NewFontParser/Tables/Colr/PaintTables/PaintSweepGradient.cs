using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintSweepGradient : IPaintTable
    {
        public byte Format => 8;

        public ColorLine ColorLine { get; }

        public short CenterX { get; }

        public short CenterY { get; }

        public float StartAngle { get; }

        public float EndAngle { get; }

        public PaintSweepGradient(BigEndianReader reader)
        {
            uint colorLineOffset = reader.ReadUInt24();
            CenterX = reader.ReadShort();
            CenterY = reader.ReadShort();
            StartAngle = reader.ReadF2Dot14();
            EndAngle = reader.ReadF2Dot14();
            reader.Seek(colorLineOffset);
            ColorLine = new ColorLine(reader);
        }
    }
}