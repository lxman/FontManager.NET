using System;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedClassSequenceRule
    {
        public ushort[] BacktrackSequences { get; }

        public ushort[] InputSequences { get; }

        public ushort[] LookaheadSequences { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public ChainedClassSequenceRule(BigEndianReader reader)
        {
            ushort backtrackGlyphCount = reader.ReadUShort();
            BacktrackSequences = reader.ReadUShortArray(backtrackGlyphCount);
            ushort inputGlyphCount = reader.ReadUShort();
            InputSequences = reader.ReadUShortArray(Convert.ToUInt32(inputGlyphCount - 1));
            ushort lookaheadGlyphCount = reader.ReadUShort();
            LookaheadSequences = reader.ReadUShortArray(lookaheadGlyphCount);
            ushort seqLookupCount = reader.ReadUShort();
            SequenceLookups = new SequenceLookup[seqLookupCount];
            for (var i = 0; i < seqLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}