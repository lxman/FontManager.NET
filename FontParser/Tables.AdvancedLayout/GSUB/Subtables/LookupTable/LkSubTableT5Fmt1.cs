using System;
using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// 5.1 Context Substitution Format 1: Simple Glyph Contexts
    /// </summary>
    internal class LkSubTableT5Fmt1 : LookupSubTable
    {
        public CoverageTable.CoverageTable coverageTable;
        public LkSubT5Fmt1_SubRuleSet[] subRuleSets;

        //5.1 Context Substitution Format 1: Simple Glyph Contexts
        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            throw new NotImplementedException();
        }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int coverage_pos = coverageTable.FindPosition(glyphIndices[pos]);
            if (coverage_pos < 0) { return false; }

            Utils.WarnUnimplemented("GSUB," + nameof(LkSubTableT5Fmt1));
            return false;
        }
    }
}