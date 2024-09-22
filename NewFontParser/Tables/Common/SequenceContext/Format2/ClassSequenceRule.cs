using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class ClassSequenceRule
    {
        public ushort[]? InputSequences { get; }

        public SequenceLookup[]? SequenceLookups { get; }

        public ClassSequenceRule(BigEndianReader reader)
        {
            ushort glyphCount = reader.ReadUShort();
            ushort sequenceLookupCount = reader.ReadUShort();
            if (glyphCount == 0 && sequenceLookupCount == 0)
            {
                return;
            }

            InputSequences = new ushort[glyphCount - 1];
            for (var i = 0; i < glyphCount - 1; i++)
            {
                InputSequences[i] = reader.ReadUShort();
            }

            SequenceLookups = new SequenceLookup[sequenceLookupCount];
            for (var i = 0; i < sequenceLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}