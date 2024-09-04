using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;
using Serilog;

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
            Log.Debug($"BacktrackGlyphCount = {BacktrackGlyphCount}");
            BacktrackSequence = reader.ReadUShortArray(BacktrackGlyphCount);
            InputGlyphCount = reader.ReadUShort();
            Log.Debug($"InputGlyphCount = {InputGlyphCount}");
            InputSequence = reader.ReadUShortArray(InputGlyphCount);
            LookaheadGlyphCount = reader.ReadUShort();
            Log.Debug($"LookaheadGlyphCount = {LookaheadGlyphCount}");
            LookaheadSequence = reader.ReadUShortArray(LookaheadGlyphCount);
            SeqLookupCount = reader.ReadUShort();
            Log.Debug($"SeqLookupCount = {SeqLookupCount}");
            SequenceLookups = new SequenceLookup[SeqLookupCount];
            for (var i = 0; i < SeqLookupCount; i++)
            {
                Log.Debug("Creating SequenceLookup");
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
                Log.Debug("Creating SequenceLookup success!");
            }
        }
    }
}
