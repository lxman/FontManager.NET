using System;
using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    //-----------------------------------------------------------------
    //https://docs.microsoft.com/en-us/typography/opentype/otspec180/gpos#lookup-type-6--marktomark-attachment-positioning-subtable
    /// <summary>
    /// Lookup Type 6: MarkToMark Attachment
    /// defines the position of one mark relative to another mark
    /// </summary>
    internal class LkSubTableType6 : LookupSubTable
    {
        public CoverageTable.CoverageTable MarkCoverage1 { get; set; }
        public CoverageTable.CoverageTable MarkCoverage2 { get; set; }
        public MarkArrayTable Mark1ArrayTable { get; set; }
        public Mark2ArrayTable Mark2ArrayTable { get; set; } // Mark2 attachment points used to attach Mark1 glyphs to a specific Mark2 glyph.

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            //The attaching mark is Mark1,
            //and the base mark being attached to is Mark2.

            //The Mark2 glyph (that combines with a Mark1 glyph) is the glyph preceding the Mark1 glyph in glyph string order
            //(skipping glyphs according to LookupFlags)

            //@prepare: we must found mark2 glyph before mark1
            bool longLookBack = OwnerGPos.EnableLongLookBack;
#if DEBUG
            if (len == 3 || len == 4)
            {
            }
#endif
            //find marker
            int lim = Math.Min(startAt + len, inputGlyphs.Count);

            for (int i = Math.Max(startAt, 1); i < lim; ++i)
            {
                // Find first mark glyph
                int mark1Found = MarkCoverage1.FindPosition(inputGlyphs.GetGlyph(i, out short glyph_adv_w));
                if (mark1Found < 0)
                {
                    continue;
                }

                // Look back for previous mark glyph
                int prevMark = LookupTable.FindGlyphBackwardByKind(inputGlyphs, GlyphClassKind.Mark, i, longLookBack ? startAt : i - 1);
                if (prevMark < 0)
                {
                    continue;
                }

                int mark2Found = MarkCoverage2.FindPosition(inputGlyphs.GetGlyph(prevMark, out short prevPosAdvW));
                if (mark2Found < 0)
                {
                    continue;
                }

                // Examples:
                // 👨🏻‍👩🏿‍👧🏽‍👦🏽‍👦🏿 in Segoe UI Emoji

                int mark1ClassId = Mark1ArrayTable.GetMarkClass(mark1Found);
                AnchorPoint prevAnchor = Mark2ArrayTable.GetAnchorPoint(mark2Found, mark1ClassId);
                AnchorPoint anchor = Mark1ArrayTable.GetAnchorPoint(mark1Found);
                if (anchor.ycoord < 0)
                {
                    //temp HACK!   น้ำ in Tahoma
                    inputGlyphs.AppendGlyphOffset(prevMark /*PREV*/, anchor.xcoord, anchor.ycoord);
                }
                else
                {
                    inputGlyphs.GetOffset(prevMark, out short prevGlyphXoffset, out short prevGlyphYoffset);
                    inputGlyphs.GetOffset(i, out short glyphXoffset, out short glyphYoffset);
                    int xoffset = prevGlyphXoffset + prevAnchor.xcoord - (prevPosAdvW + glyphXoffset + anchor.xcoord);
                    int yoffset = prevGlyphYoffset + prevAnchor.ycoord - (glyphYoffset + anchor.ycoord);
                    inputGlyphs.AppendGlyphOffset(i, (short)xoffset, (short)yoffset);
                }
            }
        }
    }
}