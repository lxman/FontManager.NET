using NewFontParser.Reader;
using NewFontParser.Tables.Proprietary.Aat.Morx.StateTables;

namespace NewFontParser.Tables.Proprietary.Aat.Kerx.Subtables
{
    public class Format4 : IKerxSubtable
    {
        public uint Length { get; }

        public KerxCoverage Coverage { get; }

        public uint TupleCount { get; }

        public StxHeader Header { get; }

        public uint Flags { get; }

        public Format4(BigEndianReader reader)
        {
            Length = reader.ReadUInt32();
            Coverage = (KerxCoverage)reader.ReadUInt32();
            TupleCount = reader.ReadUInt32();
            Header = new StxHeader(reader);
            Flags = reader.ReadUInt32();
        }
    }
}
