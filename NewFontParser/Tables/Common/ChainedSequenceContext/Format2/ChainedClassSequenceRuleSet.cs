using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedClassSequenceRuleSet
    {
        public List<ChainedClassSequenceRule> ChainedClassSequenceRules { get; } = new List<ChainedClassSequenceRule>();

        public ChainedClassSequenceRuleSet(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort chainedClassSequenceRuleCount = reader.ReadUShort();
            ushort[] chainedClassSequenceRuleOffsets = reader.ReadUShortArray(chainedClassSequenceRuleCount);
            for (var i = 0; i < chainedClassSequenceRuleCount; i++)
            {
                reader.Seek(position + chainedClassSequenceRuleOffsets[i]);
                ChainedClassSequenceRules.Add(new ChainedClassSequenceRule(reader));
            }
        }
    }
}