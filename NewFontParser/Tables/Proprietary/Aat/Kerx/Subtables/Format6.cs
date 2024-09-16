using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Kerx.Subtables
{
    public class Format6 : IKerxSubtable
    {
        public uint Length { get; }

        public KerxCoverage Coverage { get; }

        public uint TupleCount { get; }

        public uint Flags { get; }

        public ushort RowCount { get; }

        public ushort ColumnCount { get; }

        public uint RowIndexTableOffset { get; }

        public uint ColumnIndexTableOffset { get; }

        public uint KerningArrayOffset { get; }

        public uint KerningVectorOffset { get; }

        public Format6(BigEndianReader reader)
        {
            Length = reader.ReadUInt32();
            Coverage = (KerxCoverage)reader.ReadUInt32();
            TupleCount = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            RowCount = reader.ReadUShort();
            ColumnCount = reader.ReadUShort();
            RowIndexTableOffset = reader.ReadUInt32();
            ColumnIndexTableOffset = reader.ReadUInt32();
            KerningArrayOffset = reader.ReadUInt32();
            KerningVectorOffset = reader.ReadUInt32();
        }
    }
}