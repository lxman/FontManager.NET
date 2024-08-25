using System;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class Format1 : ILookupSubTable
    {
        public ushort PosFormat { get; }

        public ushort CoverageOffset { get; }

        public ValueFormat ValueFormat1 { get; }

        public ValueFormat ValueFormat2 { get; }

        public ushort PairSetCount { get; }

        public ushort[] PairSetOffsets { get; }

        public PairSet[] PairSets { get; }

        public Format1(BigEndianReader reader)
        {
            long position = reader.Position;
            PosFormat = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ValueFormat1 = (ValueFormat)reader.ReadShort();
            ValueFormat2 = (ValueFormat)reader.ReadShort();
            PairSetCount = reader.ReadUShort();

            PairSetOffsets = new ushort[PairSetCount];
            for (var i = 0; i < PairSetCount; i++)
            {
                PairSetOffsets[i] = reader.ReadUShort();
            }

            reader.Seek(position);
            byte[] data = reader.PeekBytes(Convert.ToInt32(reader.BytesRemaining));
            var reader2 = new BigEndianReader(data);
            var tables = new ReadSubTablesFromOffset16Array<PairSet>(reader2, PairSetOffsets);
            PairSets = tables.Tables;
        }
    }
}
