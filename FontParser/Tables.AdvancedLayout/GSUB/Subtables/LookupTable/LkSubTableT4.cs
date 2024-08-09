using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LkSubTableT4 : LookupSubTable
    {
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public LigatureSetTable[] LigatureSetTables { get; set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            //check coverage
            ushort glyphIndex = glyphIndices[pos];
            int foundPos = CoverageTable.FindPosition(glyphIndex);
            if (foundPos > -1)
            {
                LigatureSetTable ligTable = LigatureSetTables[foundPos];
                foreach (LigatureTable lig in ligTable.Ligatures)
                {
                    int remainingLen = len - 1;
                    int compLen = lig.ComponentGlyphs.Length;
                    if (compLen > remainingLen)
                    {   // skip tp next component
                        continue;
                    }
                    var allMatched = true;
                    int tmp_i = pos + 1;
                    for (var p = 0; p < compLen; ++p)
                    {
                        if (glyphIndices[tmp_i + p] != lig.ComponentGlyphs[p])
                        {
                            allMatched = false;
                            break; //exit from loop
                        }
                    }
                    if (allMatched)
                    {
                        // remove all matches and replace with selected glyph
                        glyphIndices.Replace(pos, compLen + 1, lig.GlyphId);
                        return true;
                    }
                }
            }
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            foreach (ushort glyphIndex in CoverageTable.GetExpandedValueIter())
            {
                int foundPos = CoverageTable.FindPosition(glyphIndex);
                LigatureSetTable ligTable = LigatureSetTables[foundPos];
                foreach (LigatureTable lig in ligTable.Ligatures)
                {
                    outputAssocGlyphs.Add(lig.GlyphId);
                }
            }
        }
    }
}