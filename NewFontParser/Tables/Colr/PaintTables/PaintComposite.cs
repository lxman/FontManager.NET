using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintComposite : IPaintTable
    {
        public byte Format => 32;

        public IPaintTable SourceTable { get; }

        public IPaintTable SubTable { get; }

        public CompositeMode CompositeMode { get; }

        public PaintComposite(BigEndianReader reader)
        {
            uint sourceTableOffset = reader.ReadUInt24();
            CompositeMode = (CompositeMode)reader.ReadByte();
            uint subTableOffset = reader.ReadUInt24();
            SourceTable = PaintTableFactory.CreatePaintTable(reader, sourceTableOffset);
            SubTable = PaintTableFactory.CreatePaintTable(reader, subTableOffset);
        }
    }
}