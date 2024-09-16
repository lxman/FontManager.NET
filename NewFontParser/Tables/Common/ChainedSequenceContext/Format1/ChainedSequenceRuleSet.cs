using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format1
{
    public class ChainedSequenceRuleSet
    {
        public ChainedSequenceRule[] ChainedSequenceRules { get; }

        public ChainedSequenceRuleSet(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            ushort chainedSequenceRuleCount = reader.ReadUShort();
            ushort[] chainedSequenceRuleOffsets = reader.ReadUShortArray(chainedSequenceRuleCount);
            ChainedSequenceRules = new ChainedSequenceRule[chainedSequenceRuleCount];
            for (var i = 0; i < chainedSequenceRuleCount; i++)
            {
                reader.Seek(startOfTable + chainedSequenceRuleOffsets[i]);
                ChainedSequenceRules[i] = new ChainedSequenceRule(reader);
            }
        }
    }
}