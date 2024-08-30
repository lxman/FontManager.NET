using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintTransform : IPaintTable
    {
        public byte Format => 12;

        public IPaintTable SubTable { get; }

        public Affine2X3 Transform { get; }

        public PaintTransform(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            uint transformOffset = reader.ReadUInt24();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
            reader.Seek(transformOffset);
            Transform = new Affine2X3(reader);
        }
    }
}
