using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.ClassDefinition;
using NewFontParser.Tables.Common.CoverageFormat;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace NewFontParser.Tables.Common.SequenceContext.Format2
{
    public class SequenceContextFormat2 : ILookupSubTable
    {
        public ushort Format { get; }

        public List<ClassSequenceRuleSet> ClassSequenceRuleSets { get; } = new List<ClassSequenceRuleSet>();

        public ICoverageFormat Coverage { get; }

        public IClassDefinition ClassDef { get; }

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
            reader.Seek(startOfTable + coverageOffset);
            ushort coverageFormat = reader.PeekBytes(2)[1];
            Coverage = coverageFormat switch
            {
                1 => new CoverageFormat.Format1(reader),
                2 => new CoverageFormat.Format2(reader),
                _ => Coverage
            };
            reader.Seek(startOfTable + classDefOffset);
            coverageFormat = reader.PeekBytes(2)[1];
            ClassDef = coverageFormat switch
            {
                1 => new ClassDefinition.Format1(reader),
                2 => new ClassDefinition.Format2(reader),
                _ => ClassDef
            };
        }
    }
}