using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class SequenceContextFormat2 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort CoverageOffset { get; }

        public ushort ClassDefOffset { get; }

        public ushort ClassSequenceRuleSetCount { get; }

        public ClassSequenceRuleSet[] ClassSequenceRuleSets { get; }

        public SequenceContextFormat2(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ClassDefOffset = reader.ReadUShort();
            ClassSequenceRuleSetCount = reader.ReadUShort();

            ClassSequenceRuleSets = new ClassSequenceRuleSet[ClassSequenceRuleSetCount];
            for (var i = 0; i < ClassSequenceRuleSetCount; i++)
            {
                reader.Seek(ClassDefOffset);
                ClassSequenceRuleSets[i] = new ClassSequenceRuleSet(reader);
            }
        }
    }
}
