using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarLinearGradient : IPaintTable
    {
        public byte Format => 5;

        public ColorLine ColorLine { get; }

        public short X0 { get; }

        public short Y0 { get; }

        public short X1 { get; }

        public short Y1 { get; }

        public short X2 { get; }

        public short Y2 { get; }

        public uint VarIndexBase { get; }

        public PaintVarLinearGradient(BigEndianReader reader)
        {
            long position = reader.Position;

            uint colorLineOffset = reader.ReadUInt24();
            X0 = reader.ReadShort();
            Y0 = reader.ReadShort();
            X1 = reader.ReadShort();
            Y1 = reader.ReadShort();
            X2 = reader.ReadShort();
            Y2 = reader.ReadShort();
            VarIndexBase = reader.ReadUInt32();
            reader.Seek(position + colorLineOffset);
            ColorLine = new ColorLine(reader);
        }
    }
}
