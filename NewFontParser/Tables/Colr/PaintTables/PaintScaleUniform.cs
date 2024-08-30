using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintScaleUniform : IPaintTable
    {
        public byte Format => 20;

        public IPaintTable SubTable { get; }

        public float Scale { get; }

        public PaintScaleUniform(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            Scale = reader.ReadF2Dot14();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}
