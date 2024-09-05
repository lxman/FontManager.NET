using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarTranslate : IPaintTable
    {
        public byte Format => 15;

        public IPaintTable SubTable { get; }

        public short Dx { get; }

        public short Dy { get; }

        public uint VarIndexBase { get; }

        public PaintVarTranslate(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            Dx = reader.ReadShort();
            Dy = reader.ReadShort();
            VarIndexBase = reader.ReadUInt32();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}