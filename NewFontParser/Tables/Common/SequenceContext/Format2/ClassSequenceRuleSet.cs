using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class ClassSequenceRuleSet
    {
        public ushort ClassSeqRuleCount { get; }

        public ushort[] ClassSeqRuleOffsets { get; }

        public ClassSequenceRule[] ClassSeqRules { get; }

        public ClassSequenceRuleSet(BigEndianReader reader)
        {
            ClassSeqRuleCount = reader.ReadUShort();
            ClassSeqRuleOffsets = new ushort[ClassSeqRuleCount];

            for (var i = 0; i < ClassSeqRuleCount; i++)
            {
                ClassSeqRuleOffsets[i] = reader.ReadUShort();
            }

            ClassSeqRules = new ClassSequenceRule[ClassSeqRuleCount];
            for (var i = 0; i < ClassSeqRuleCount; i++)
            {
                reader.Seek(ClassSeqRuleOffsets[i]);
                ClassSeqRules[i] = new ClassSequenceRule(reader);
            }
        }
    }
}