﻿using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class LkSubTableType7Fmt1 : LookupSubTable
    {
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public PosRuleSetTable[] PosRuleSetTables { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            Utils.WarnUnimplemented("GPOS Lookup Sub Table Type 7 Format 1");
        }
    }
}