using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format3
{
    public class ChainedSequenceContextFormat3 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort BacktrackGlyphCount { get; }

        public ushort[] BacktrackCoverageOffsets { get; }

        public ushort InputGlyphCount { get; }

        public ushort[] InputCoverageOffsets { get; }

        public ushort LookaheadGlyphCount { get; }

        public ushort[] LookaheadCoverageOffsets { get; }

        public ushort SequenceLookupCount { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public ChainedSequenceContextFormat3(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            BacktrackGlyphCount = reader.ReadUShort();
            BacktrackCoverageOffsets = reader.ReadUShortArray(BacktrackGlyphCount);
            InputGlyphCount = reader.ReadUShort();
            InputCoverageOffsets = reader.ReadUShortArray(InputGlyphCount);
            LookaheadGlyphCount = reader.ReadUShort();
            LookaheadCoverageOffsets = reader.ReadUShortArray(LookaheadGlyphCount);
            SequenceLookupCount = reader.ReadUShort();
            SequenceLookups = new SequenceLookup[SequenceLookupCount];
            for (var i = 0; i < SequenceLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}