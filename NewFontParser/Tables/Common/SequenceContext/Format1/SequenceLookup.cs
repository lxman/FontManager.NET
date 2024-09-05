using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceLookup
    {
        public ushort LookupCount { get; }
        public ushort LookupListIndex { get; }

        public SequenceLookup(byte[] data)
        {
            var reader = new BigEndianReader(data);

            LookupCount = reader.ReadUShort();
            LookupListIndex = reader.ReadUShort();
        }
    }
}