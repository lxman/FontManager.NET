using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class ClassSequenceRuleSet
    {
        public ClassSequenceRule[] ClassSeqRules { get; }

        public ClassSequenceRuleSet(BigEndianReader reader)
        {
            long start = reader.Position;

            ushort classSeqRuleCount = reader.ReadUShort();
            ushort[] classSeqRuleOffsets = reader.ReadUShortArray(classSeqRuleCount);

            ClassSeqRules = new ClassSequenceRule[classSeqRuleCount];
            for (var i = 0; i < classSeqRuleCount; i++)
            {
                reader.Seek(start + classSeqRuleOffsets[i]);
                ClassSeqRules[i] = new ClassSequenceRule(reader);
            }
        }
    }
}