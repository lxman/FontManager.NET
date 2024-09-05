using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarTransform : IPaintTable
    {
        public byte Format => 12;

        public IPaintTable SubTable { get; }

        public VarAffine2X3 Transform { get; }

        public PaintVarTransform(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            uint transformOffset = reader.ReadUInt24();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
            reader.Seek(transformOffset);
            Transform = new VarAffine2X3(reader);
        }
    }
}