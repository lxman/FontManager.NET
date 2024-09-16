using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.SequenceContext.Format3
{
    public class SequenceContextFormat3 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort[] CoverageOffsets { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public SequenceContextFormat3(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            ushort glyphCount = reader.ReadUShort();
            ushort seqLookupCount = reader.ReadUShort();
            CoverageOffsets = reader.ReadUShortArray(glyphCount);
            SequenceLookups = new SequenceLookup[seqLookupCount];
            for (var i = 0; i < seqLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}