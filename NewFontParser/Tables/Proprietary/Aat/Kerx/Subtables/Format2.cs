using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Kerx.Subtables
{
    public class Format2 : IKerxSubtable
    {
        public uint Length { get; }

        public KerxCoverage Coverage { get; }

        public uint TupleCount { get; }

        public uint RowWidth { get; }

        public uint LeftOffsetTableOffset { get; }

        public uint RightOffsetTableOffset { get; }

        public uint KerningArrayOffset { get; }

        public Format2(BigEndianReader reader)
        {
            Length = reader.ReadUInt32();
            Coverage = (KerxCoverage)reader.ReadUInt32();
            TupleCount = reader.ReadUInt32();
            RowWidth = reader.ReadUInt32();
            LeftOffsetTableOffset = reader.ReadUInt32();
            RightOffsetTableOffset = reader.ReadUInt32();
            KerningArrayOffset = reader.ReadUInt32();
        }
    }
}