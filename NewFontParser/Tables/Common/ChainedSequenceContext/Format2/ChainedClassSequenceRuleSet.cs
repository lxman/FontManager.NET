using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedClassSequenceRuleSet
    {
        public ushort ChainedClassSequenceRuleCount { get; }

        public ChainedClassSequenceRule[] ChainedClassSequenceRules { get; }

        public ChainedClassSequenceRuleSet(BigEndianReader reader)
        {
            ChainedClassSequenceRuleCount = reader.ReadUShort();
            ChainedClassSequenceRules = new ChainedClassSequenceRule[ChainedClassSequenceRuleCount];
            for (var i = 0; i < ChainedClassSequenceRuleCount; i++)
            {
                ChainedClassSequenceRules[i] = new ChainedClassSequenceRule(reader);
            }
        }
    }
}
