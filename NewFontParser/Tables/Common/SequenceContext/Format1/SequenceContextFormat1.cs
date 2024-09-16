using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceContextFormat1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat Coverage { get; }

        public ushort[] RuleSetOffsets { get; }

        public SequenceRuleSet[] SequenceRuleSets { get; }

        public SequenceContextFormat1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort ruleSetCount = reader.ReadUShort();
            RuleSetOffsets = new ushort[ruleSetCount];

            for (var i = 0; i < ruleSetCount; i++)
            {
                RuleSetOffsets[i] = reader.ReadUShort();
            }

            SequenceRuleSets = new SequenceRuleSet[ruleSetCount];

            for (var i = 0; i < ruleSetCount; i++)
            {
                reader.Seek(startOfTable + RuleSetOffsets[i]);
                SequenceRuleSets[i] = new SequenceRuleSet(reader);
            }

            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}