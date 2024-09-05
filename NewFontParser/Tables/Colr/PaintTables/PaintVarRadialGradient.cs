using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarRadialGradient : IPaintTable
    {
        public byte Format => 7;

        public ColorLine ColorLine { get; }

        public short X0 { get; }

        public short Y0 { get; }

        public ushort Radius0 { get; }

        public short X1 { get; }

        public short Y1 { get; }

        public ushort Radius1 { get; }

        public uint VarIndexBase { get; }

        public PaintVarRadialGradient(BigEndianReader reader)
        {
            uint colorLineOffset = reader.ReadUInt24();
            X0 = reader.ReadShort();
            Y0 = reader.ReadShort();
            Radius0 = reader.ReadUShort();
            X1 = reader.ReadShort();
            Y1 = reader.ReadShort();
            Radius1 = reader.ReadUShort();
            VarIndexBase = reader.ReadUInt24();
            reader.Seek(colorLineOffset);
            ColorLine = new ColorLine(reader);
        }
    }
}