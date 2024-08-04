using System;
using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class LkSubTableType1 : LookupSubTable
    {
        public LkSubTableType1(CoverageTable.CoverageTable coverage, ValueRecord singleValue)
        {
            Format = 1;
            _coverageTable = coverage;
            _valueRecords = new[] { singleValue };
        }

        public LkSubTableType1(CoverageTable.CoverageTable coverage, ValueRecord[] valueRecords)
        {
            Format = 2;
            _coverageTable = coverage;
            _valueRecords = valueRecords;
        }

        public int Format { get; }
        private readonly CoverageTable.CoverageTable _coverageTable;
        private readonly ValueRecord[] _valueRecords;

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            int lim = Math.Min(startAt + len, inputGlyphs.Count);
            for (int i = startAt; i < lim; ++i)
            {
                ushort glyph_index = inputGlyphs.GetGlyph(i, out short glyph_advW);
                int cov_index = _coverageTable.FindPosition(glyph_index);
                if (cov_index > -1)
                {
                    var vr = _valueRecords[Format == 1 ? 0 : cov_index];
                    inputGlyphs.AppendGlyphOffset(i, vr.XPlacement, vr.YPlacement);
                    inputGlyphs.AppendGlyphAdvance(i, vr.XAdvance, 0);
                }
            }
        }
    }
}