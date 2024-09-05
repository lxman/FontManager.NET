using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class PairSet
    {
        public ushort PairValueCount { get; }

        public PairValueRecord[] PairValueRecords { get; }

        public PairSet(BigEndianReader reader)
        {
            PairValueCount = reader.ReadUShort();
            PairValueRecords = new PairValueRecord[PairValueCount];
            for (var i = 0; i < PairValueCount; i++)
            {
                PairValueRecords[i] = new PairValueRecord(reader.ReadBytes(6));
            }
        }
    }
}