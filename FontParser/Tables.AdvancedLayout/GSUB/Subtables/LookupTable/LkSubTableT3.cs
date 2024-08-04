using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// LookupType 3: Alternate Substitution Subtable
    /// </summary>
    internal class LkSubTableT3 : LookupSubTable
    {
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public AlternativeSetTable[] AlternativeSetTables { get; set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            //Coverage table containing the indices of glyphs with alternative forms(Coverage),
            int isCovered = CoverageTable.FindPosition(glyphIndices[pos]);
            //this.CoverageTable.FindPosition()
            Utils.WarnUnimplemented("GSUB, Lookup Subtable Type 3");
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            Utils.WarnUnimplementedCollectAssocGlyphs(ToString());
        }
    }
}