using System;
using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    internal class LkSubTableType8Fmt3 : LookupSubTable
    {
        public CoverageTable.CoverageTable[] BacktrackCoverages { get; set; }
        public CoverageTable.CoverageTable[] InputGlyphCoverages { get; set; }
        public CoverageTable.CoverageTable[] LookaheadCoverages { get; set; }
        public PosLookupRecord[] PosLookupRecords { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            startAt = Math.Max(startAt, BacktrackCoverages.Length);
            int lim = Math.Min(startAt + len, inputGlyphs.Count) - (InputGlyphCoverages.Length - 1) - LookaheadCoverages.Length;
            for (int pos = startAt; pos < lim; ++pos)
            {
                DoGlyphPositionAt(inputGlyphs, pos);
            }
        }

        protected void DoGlyphPositionAt(IGlyphPositions inputGlyphs, int pos)
        {
            // Check all coverages: if any of them does not match, abort substitution
            for (int i = 0; i < InputGlyphCoverages.Length; ++i)
            {
                if (InputGlyphCoverages[i].FindPosition(inputGlyphs.GetGlyph(pos + i, out var unused)) < 0)
                {
                    return;
                }
            }

            for (int i = 0; i < BacktrackCoverages.Length; ++i)
            {
                if (BacktrackCoverages[i].FindPosition(inputGlyphs.GetGlyph(pos - 1 - i, out var unused)) < 0)
                {
                    return;
                }
            }

            for (int i = 0; i < LookaheadCoverages.Length; ++i)
            {
                if (LookaheadCoverages[i].FindPosition(inputGlyphs.GetGlyph(pos + InputGlyphCoverages.Length + i, out var unused)) < 0)
                {
                    return;
                }
            }

            foreach (var plr in PosLookupRecords)
            {
                var lookup = OwnerGPos.LookupList[plr.lookupListIndex];
                lookup.DoGlyphPosition(inputGlyphs, pos + plr.seqIndex, InputGlyphCoverages.Length - plr.seqIndex);
            }
        }
    }
}