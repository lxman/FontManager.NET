using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class LkSubTableType7Fmt3 : LookupSubTable
    {
        public CoverageTable.CoverageTable[] CoverageTables { get; set; }
        public PosLookupRecord[] PosLookupRecords { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            Utils.WarnUnimplemented("GPOS Lookup Sub Table Type 7 Format 3");
        }
    }
}