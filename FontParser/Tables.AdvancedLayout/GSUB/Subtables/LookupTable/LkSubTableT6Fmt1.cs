using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LkSubTableT6Fmt1 : LookupSubTable
    {
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public ChainSubRuleSetTable[] SubRuleSets { get; set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            Utils.WarnUnimplemented("GSUB, Lookup Subtable Type 6 Format 1");
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            Utils.WarnUnimplementedCollectAssocGlyphs(ToString());
        }
    }
}