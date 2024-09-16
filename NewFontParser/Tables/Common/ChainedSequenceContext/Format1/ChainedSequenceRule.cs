using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format1
{
    public class ChainedSequenceRule
    {
        public ushort[] BacktrackSequence { get; }

        public ushort[] InputSequence { get; }

        public ushort[] LookaheadSequence { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public ChainedSequenceRule(BigEndianReader reader)
        {
            ushort backtrackGlyphCount = reader.ReadUShort();
            BacktrackSequence = reader.ReadUShortArray(backtrackGlyphCount);
            ushort inputGlyphCount = reader.ReadUShort();
            InputSequence = reader.ReadUShortArray(inputGlyphCount - 1);
            ushort lookaheadGlyphCount = reader.ReadUShort();
            LookaheadSequence = reader.ReadUShortArray(lookaheadGlyphCount);
            ushort seqLookupCount = reader.ReadUShort();
            SequenceLookups = new SequenceLookup[seqLookupCount];
            for (var i = 0; i < seqLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}