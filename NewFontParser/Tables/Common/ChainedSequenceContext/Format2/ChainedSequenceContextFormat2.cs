using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedSequenceContextFormat2 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort CoverageOffset { get; }

        public ushort BacktrackClassDefOffset { get; }

        public ushort InputClassDefOffset { get; }

        public ushort LookaheadClassDefOffset { get; }

        public ushort ChainedClassSequenceRuleSetCount { get; }

        public ChainedClassSequenceRuleSet[] ChainedClassSequenceRuleSets { get; }

        public ChainedSequenceContextFormat2(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            BacktrackClassDefOffset = reader.ReadUShort();
            InputClassDefOffset = reader.ReadUShort();
            LookaheadClassDefOffset = reader.ReadUShort();
            ChainedClassSequenceRuleSetCount = reader.ReadUShort();
            ChainedClassSequenceRuleSets = new ChainedClassSequenceRuleSet[ChainedClassSequenceRuleSetCount];
            for (var i = 0; i < ChainedClassSequenceRuleSetCount; i++)
            {
                ChainedClassSequenceRuleSets[i] = new ChainedClassSequenceRuleSet(reader);
            }
        }
    }
}
