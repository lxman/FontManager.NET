using NewFontParser.Reader;
using NewFontParser.Tables.Proprietary.Aat.Morx.StateTables;

namespace NewFontParser.Tables.Proprietary.Aat.Kerx.Subtables
{
    public class KerxSubtablesFormat1 : IKerxSubtable
    {
        public uint Length { get; }

        public KerxCoverage Coverage { get; }

        public uint TupleCount { get; }

        public StxHeader Header { get; }

        public uint ValueTableOffset { get; }

        public KerxSubtablesFormat1(BigEndianReader reader)
        {
            Length = reader.ReadUInt32();
            Coverage = (KerxCoverage)reader.ReadUInt32();
            TupleCount = reader.ReadUInt32();
            Header = new StxHeader(reader);
            ValueTableOffset = reader.ReadUInt32();
        }
    }
}