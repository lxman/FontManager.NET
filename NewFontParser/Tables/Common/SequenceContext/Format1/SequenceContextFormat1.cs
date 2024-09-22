using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceContextFormat1 : ILookupSubTable, ISequenceContext
    {
        public ICoverageFormat Coverage { get; }

        public SequenceRuleSet[] SequenceRuleSets { get; }

        public SequenceContextFormat1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            _ = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort ruleSetCount = reader.ReadUShort();
            ushort[] ruleSetOffsets = reader.ReadUShortArray(ruleSetCount);
            SequenceRuleSets = new SequenceRuleSet[ruleSetCount];
            for (var i = 0; i < ruleSetCount; i++)
            {
                reader.Seek(startOfTable + ruleSetOffsets[i]);
                SequenceRuleSets[i] = new SequenceRuleSet(reader);
            }

            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}