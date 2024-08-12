using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class LkSubTableType8Fmt2 : LookupSubTable
    {
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public PosClassSetTable[] PosClassSetTables { get; set; }

        public ClassDefTable.ClassDefTable BackTrackClassDef { get; set; }
        public ClassDefTable.ClassDefTable InputClassDef { get; set; }
        public ClassDefTable.ClassDefTable LookaheadClassDef { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            ushort glyphIndex = inputGlyphs.GetGlyph(startAt, out short advW);

            int coverage_pos = CoverageTable.FindPosition(glyphIndex);
            if (coverage_pos < 0) { return; }

            Utils.WarnUnimplemented("GPOS Lookup Sub Table Type 8 Format 2");
        }
    }
}