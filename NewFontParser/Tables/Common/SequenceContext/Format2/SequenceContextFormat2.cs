using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class SequenceContextFormat2 : ILookupSubTable
    {
        public ushort Format { get; }

        public List<ClassSequenceRuleSet> ClassSequenceRuleSets { get; } = new List<ClassSequenceRuleSet>();

        public SequenceContextFormat2(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort classDefOffset = reader.ReadUShort();
            ushort classSequenceRuleSetCount = reader.ReadUShort();
            ushort[] classSequenceRuleSetOffsets = reader.ReadUShortArray(classSequenceRuleSetCount);

            for (var i = 0; i < classSequenceRuleSetCount; i++)
            {
                if (classSequenceRuleSetOffsets[i] == 0)
                {
                    continue;
                }
                reader.Seek(startOfTable + classSequenceRuleSetOffsets[i]);
                ClassSequenceRuleSets.Add(new ClassSequenceRuleSet(reader));
            }
        }
    }
}