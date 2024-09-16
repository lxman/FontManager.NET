using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceRuleSet
    {
        public SequenceRule[] SequenceRules { get; }

        public SequenceRuleSet(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            ushort ruleCount = reader.ReadUShort();
            ushort[] ruleOffsets = reader.ReadUShortArray(ruleCount);
            SequenceRules = new SequenceRule[ruleCount];

            for (var i = 0; i < ruleCount; i++)
            {
                reader.Seek(startOfTable + ruleOffsets[i]);
                SequenceRules[i] = new SequenceRule(reader);
            }
        }
    }
}