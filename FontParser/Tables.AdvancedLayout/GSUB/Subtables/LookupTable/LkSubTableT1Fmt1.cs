using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    ///  for lookup table type 1, format1
    /// </summary>
    internal class LkSubTableT1Fmt1 : LookupSubTable
    {
        public LkSubTableT1Fmt1(CoverageTable.CoverageTable coverageTable, ushort deltaGlyph)
        {
            CoverageTable = coverageTable;
            DeltaGlyph = deltaGlyph;
        }

        /// <summary>
        /// Add to original GlyphID to get substitute GlyphID
        /// </summary>
        public ushort DeltaGlyph { get; private set; }

        public CoverageTable.CoverageTable CoverageTable { get; private set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            ushort glyphIndex = glyphIndices[pos];
            if (CoverageTable.FindPosition(glyphIndex) > -1)
            {
                glyphIndices.Replace(pos, (ushort)(glyphIndex + DeltaGlyph));
                return true;
            }
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            //1. iterate glyphs from CoverageTable
            foreach (ushort glyphIndex in CoverageTable.GetExpandedValueIter())
            {
                //2. add substitution glyph
                outputAssocGlyphs.Add((ushort)(glyphIndex + DeltaGlyph));
            }
        }
    }
}