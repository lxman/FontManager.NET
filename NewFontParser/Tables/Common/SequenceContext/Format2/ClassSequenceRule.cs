using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class ClassSequenceRule
    {
        public ushort GlyphCount { get; }

        public ushort SequenceLookupCount { get; }

        public ushort[]? InputSequences { get; }

        public SequenceLookup[]? SequenceLookups { get; }

        public ClassSequenceRule(BigEndianReader reader)
        {
            GlyphCount = reader.ReadUShort();
            SequenceLookupCount = reader.ReadUShort();
            if (GlyphCount == 0 && SequenceLookupCount == 0)
            {
                return;
            }

            InputSequences = new ushort[GlyphCount - 1];
            for (var i = 0; i < GlyphCount - 1; i++)
            {
                InputSequences[i] = reader.ReadUShort();
            }

            SequenceLookups = new SequenceLookup[SequenceLookupCount];
            for (var i = 0; i < SequenceLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}