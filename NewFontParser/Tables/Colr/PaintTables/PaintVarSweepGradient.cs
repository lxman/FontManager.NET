using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarSweepGradient : IPaintTable
    {
        public byte Format => 9;

        public ColorLine ColorLine { get; }

        public short CenterX { get; }

        public short CenterY { get; }

        public float StartAngle { get; }

        public float EndAngle { get; }

        public uint VarIndexBase { get; }

        public PaintVarSweepGradient(BigEndianReader reader)
        {
            uint colorLineOffset = reader.ReadUInt24();
            CenterX = reader.ReadShort();
            CenterY = reader.ReadShort();
            StartAngle = reader.ReadF2Dot14();
            EndAngle = reader.ReadF2Dot14();
            VarIndexBase = reader.ReadUInt32();
            reader.Seek(colorLineOffset);
            ColorLine = new ColorLine(reader);
        }
    }
}
