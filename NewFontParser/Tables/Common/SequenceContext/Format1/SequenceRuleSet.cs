using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceRuleSet
    {
        public ushort RuleCount { get; }

        public SequenceRule[] SequenceRules { get; }

        public SequenceRuleSet(byte[] data)
        {
            var reader = new BigEndianReader(data);

            RuleCount = reader.ReadUShort();
            SequenceRules = new SequenceRule[RuleCount];

            for (var i = 0; i < RuleCount; i++)
            {
                SequenceRules[i] = new SequenceRule(reader);
            }
        }
    }
}