using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedClassSequenceRule
    {
        public ushort BacktrackGlyphCount { get; }

        public ushort[] BacktrackSequence { get; }

        public ushort InputGlyphCount { get; }

        public ushort[] InputSequence { get; }

        public ushort LookaheadGlyphCount { get; }

        public ushort[] LookaheadSequence { get; }

        public ushort SeqLookupCount { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public ChainedClassSequenceRule(BigEndianReader reader)
        {
            BacktrackGlyphCount = reader.ReadUShort();
            BacktrackSequence = reader.ReadUShortArray(BacktrackGlyphCount);
            InputGlyphCount = reader.ReadUShort();
            InputSequence = reader.ReadUShortArray(InputGlyphCount);
            LookaheadGlyphCount = reader.ReadUShort();
            LookaheadSequence = reader.ReadUShortArray(LookaheadGlyphCount);
            SeqLookupCount = reader.ReadUShort();
            SequenceLookups = new SequenceLookup[SeqLookupCount];
            for (var i = 0; i < SeqLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}