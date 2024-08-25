using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format1
{
    public class ChainedSequenceRule
    {
        public ushort BacktrackGlyphCount { get; }

        public ushort[] BacktrackSequence { get; }

        public ushort InputGlyphCount { get; }

        public ushort[] InputSequence { get; }

        public ushort LookaheadGlyphCount { get; }

        public ushort[] LookaheadSequence { get; }

        public ushort SeqLookupCount { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public ChainedSequenceRule(BigEndianReader reader)
        {
            BacktrackGlyphCount = reader.ReadUShort();
            BacktrackSequence = new ushort[BacktrackGlyphCount];
            for (var i = 0; i < BacktrackGlyphCount; i++)
            {
                BacktrackSequence[i] = reader.ReadUShort();
            }

            InputGlyphCount = reader.ReadUShort();
            InputSequence = new ushort[InputGlyphCount];
            for (var i = 0; i < InputGlyphCount; i++)
            {
                InputSequence[i] = reader.ReadUShort();
            }

            LookaheadGlyphCount = reader.ReadUShort();
            LookaheadSequence = new ushort[LookaheadGlyphCount];
            for (var i = 0; i < LookaheadGlyphCount; i++)
            {
                LookaheadSequence[i] = reader.ReadUShort();
            }

            SequenceLookups = new SequenceLookup[SeqLookupCount];
            for (var i = 0; i < SeqLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}
