using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    //Lookup Type 5: MarkToLigature Attachment Positioning Subtable
    public class LkSubTableType5 : LookupSubTable
    {
        public CoverageTable.CoverageTable MarkCoverage { get; set; }
        public CoverageTable.CoverageTable LigatureCoverage { get; set; }
        public MarkArrayTable MarkArrayTable { get; set; }
        public LigatureArrayTable LigatureArrayTable { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            Utils.WarnUnimplemented("GPOS Lookup Sub Table Type 5");
        }
    }
}