using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LkSubT5Fmt2_SubClassSet
    {
        public LkSubT5Fmt2_SubClassRule[] subClassRules;

        public static LkSubT5Fmt2_SubClassSet CreateFrom(BinaryReader reader, long beginAt)
        {
            //SubClassSet subtable
            //Table 18
            //Type        Name                                      Description
            //uint16      subClassRuleCount                         Number of SubClassRule tables
            //Offset16    subClassRuleOffsets[subClassRuleCount]    Array of offsets to SubClassRule tables. Offsets are from beginning of SubClassSet, ordered by preference.

            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            var fmt2 = new LkSubT5Fmt2_SubClassSet();
            ushort subClassRuleCount = reader.ReadUInt16();
            ushort[] subClassRuleOffsets = reader.ReadUInt16Array(subClassRuleCount);
            fmt2.subClassRules = new LkSubT5Fmt2_SubClassRule[subClassRuleCount];
            for (var i = 0; i < subClassRuleCount; ++i)
            {
                var subClassRule = new LkSubT5Fmt2_SubClassRule();
                subClassRule.ReadFrom(reader, beginAt + subClassRuleOffsets[i]);
                fmt2.subClassRules[i] = subClassRule;
            }

            return fmt2;
        }
    }
}