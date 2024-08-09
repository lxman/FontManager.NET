using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    /// <summary>
    /// Lookup Type 2, Format1: Pair Adjustment Positioning Subtable
    /// </summary>
    internal class LkSubTableType2Fmt1 : LookupSubTable
    {
        internal PairSetTable[] _pairSetTables;

        public LkSubTableType2Fmt1(PairSetTable[] pairSetTables)
        {
            _pairSetTables = pairSetTables;
        }

        public CoverageTable.CoverageTable CoverageTable { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            //find marker
            CoverageTable.CoverageTable covTable = CoverageTable;
            int lim = inputGlyphs.Count - 1;
            for (var i = 0; i < lim; ++i)
            {
                int firstGlyphFound = covTable.FindPosition(inputGlyphs.GetGlyph(i, out short glyph_advW));
                if (firstGlyphFound > -1)
                {
                    //test this with Palatino A-Y sequence
                    PairSetTable pairSet = _pairSetTables[firstGlyphFound];

                    //check second glyph
                    ushort second_glyph_index = inputGlyphs.GetGlyph(i + 1, out short second_glyph_w);

                    if (pairSet.FindPairSet(second_glyph_index, out PairSet foundPairSet))
                    {
                        ValueRecord v1 = foundPairSet.value1;
                        ValueRecord v2 = foundPairSet.value2;
                        //TODO: recheck for vertical writing ... (YAdvance)
                        if (v1 != null)
                        {
                            inputGlyphs.AppendGlyphOffset(i, v1.XPlacement, v1.YPlacement);
                            inputGlyphs.AppendGlyphAdvance(i, v1.XAdvance, 0);
                        }

                        if (v2 != null)
                        {
                            inputGlyphs.AppendGlyphOffset(i + 1, v2.XPlacement, v2.YPlacement);
                            inputGlyphs.AppendGlyphAdvance(i + 1, v2.XAdvance, 0);
                        }
                    }
                }
            }
        }
    }
}