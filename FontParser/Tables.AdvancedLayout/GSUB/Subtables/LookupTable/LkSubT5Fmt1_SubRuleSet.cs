using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    internal class LkSubT5Fmt1_SubRuleSet
    {
        public LkSubT5Fmt1_SubRule[] subRules;

        public static LkSubT5Fmt1_SubRuleSet CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            LkSubT5Fmt1_SubRuleSet subRuleSet = new LkSubT5Fmt1_SubRuleSet();

            //SubRuleSet table: All contexts beginning with the same glyph
            //Table 15
            //Type        Name                  Description
            //uint16      subRuleCount          Number of SubRule tables
            //Offset16    subRuleOffsets[subRuleCount]    Array of offsets to SubRule tables. Offsets are from beginning of SubRuleSet table, ordered by preference

            ushort subRuleCount = reader.ReadUInt16();

            ushort[] subRuleOffsets = reader.ReadUInt16Array(subRuleCount);
            var subRules = new LkSubT5Fmt1_SubRule[subRuleCount];
            subRuleSet.subRules = subRules;
            for (int i = 0; i < subRuleCount; ++i)
            {
                LkSubT5Fmt1_SubRule rule = new LkSubT5Fmt1_SubRule();
                rule.ReadFrom(reader, beginAt + subRuleOffsets[i]);

                subRules[i] = rule;
            }
            return subRuleSet;
        }
    }
}