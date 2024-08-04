using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// for lookup table type 1, format2
    /// </summary>
    internal class LkSubTableT1Fmt2 : LookupSubTable
    {
        public LkSubTableT1Fmt2(CoverageTable.CoverageTable coverageTable, ushort[] substituteGlyphs)
        {
            CoverageTable = coverageTable;
            SubstituteGlyphs = substituteGlyphs;
        }

        /// <summary>
        /// It provides an array of output glyph indices (Substitute) explicitly matched to the input glyph indices specified in the Coverage table
        /// </summary>
        public ushort[] SubstituteGlyphs { get; }

        public CoverageTable.CoverageTable CoverageTable { get; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int foundAt = CoverageTable.FindPosition(glyphIndices[pos]);
            if (foundAt > -1)
            {
                glyphIndices.Replace(pos, SubstituteGlyphs[foundAt]);
                return true;
            }
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            foreach (ushort glyphIndex in CoverageTable.GetExpandedValueIter())
            {
                //2. add substitution glyph
                int foundAt = CoverageTable.FindPosition(glyphIndex);
                outputAssocGlyphs.Add(SubstituteGlyphs[foundAt]);
            }
        }
    }
}