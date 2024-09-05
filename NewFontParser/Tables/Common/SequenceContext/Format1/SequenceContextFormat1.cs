using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceContextFormat1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort CoverageOffset { get; }

        public ushort RuleSetCount { get; }

        public ushort[] RuleSetOffsets { get; }

        //public CoverageTable Coverage { get; }

        public SequenceRuleSet[] SequenceRuleSets { get; }

        public SequenceContextFormat1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            RuleSetCount = reader.ReadUShort();
            RuleSetOffsets = new ushort[RuleSetCount];

            for (var i = 0; i < RuleSetCount; i++)
            {
                RuleSetOffsets[i] = reader.ReadUShort();
            }

            //Coverage = new CoverageTable(data, CoverageOffset);
            SequenceRuleSets = new SequenceRuleSet[RuleSetCount];

            for (var i = 0; i < RuleSetCount; i++)
            {
                uint offset = RuleSetOffsets[i];
                uint length = RuleSetOffsets[i + 1] - offset;
                reader.Seek(offset);
                SequenceRuleSets[i] = new SequenceRuleSet(reader.ReadBytes(length));
            }
        }
    }
}