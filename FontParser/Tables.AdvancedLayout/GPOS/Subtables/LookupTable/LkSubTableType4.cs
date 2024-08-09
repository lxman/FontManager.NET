using System;
using System.Collections.Generic;
using FontParser.Exceptions;
using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class LkSubTableType4 : LookupSubTable
    {
        public CoverageTable.CoverageTable MarkCoverageTable { get; set; }
        public CoverageTable.CoverageTable BaseCoverageTable { get; set; }
        public BaseArrayTable BaseArrayTable { get; set; }
        public MarkArrayTable MarkArrayTable { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            int lim = Math.Min(startAt + len, inputGlyphs.Count);

            // Find the mark glyph, starting at 1
            bool longLookBack = OwnerGPos.EnableLongLookBack;
            for (int i = Math.Max(startAt, 1); i < lim; ++i)
            {
                int markFound = MarkCoverageTable.FindPosition(inputGlyphs.GetGlyph(i, out short glyph_advW));
                if (markFound < 0)
                {
                    continue;
                }

                // Look backwards for the base glyph
                int j = LookupTable.FindGlyphBackwardByKind(inputGlyphs, GlyphClassKind.Base, i, longLookBack ? startAt : i - 1);
                if (j < 0)
                {
                    // Fall back to type 0
                    j = LookupTable.FindGlyphBackwardByKind(inputGlyphs, GlyphClassKind.Zero, i, longLookBack ? startAt : i - 1);
                    if (j < 0)
                    {
                        continue;
                    }
                }

                ushort prev_glyph = inputGlyphs.GetGlyph(j, out short prev_glyph_adv_w);
                int baseFound = BaseCoverageTable.FindPosition(prev_glyph);
                if (baseFound < 0)
                {
                    continue;
                }

                BaseRecord baseRecord = BaseArrayTable.GetBaseRecords(baseFound);
                ushort markClass = MarkArrayTable.GetMarkClass(markFound);
                // find anchor on base glyph
                AnchorPoint anchor = MarkArrayTable.GetAnchorPoint(markFound);
                AnchorPoint prev_anchor = baseRecord.anchors[markClass];
                inputGlyphs.GetOffset(j, out short prev_glyph_xoffset, out short prev_glyph_yoffset);
                inputGlyphs.GetOffset(i, out short glyph_xoffset, out short glyph_yoffset);
                int xoffset = prev_glyph_xoffset + prev_anchor.xcoord - (prev_glyph_adv_w + glyph_xoffset + anchor.xcoord);
                int yoffset = prev_glyph_yoffset + prev_anchor.ycoord - (glyph_yoffset + anchor.ycoord);
                inputGlyphs.AppendGlyphOffset(i, (short)xoffset, (short)yoffset);
            }
        }

#if DEBUG

        public void dbugTest()
        {
            //count base covate
            var expandedMarks = new List<ushort>(MarkCoverageTable.GetExpandedValueIter());
            if (expandedMarks.Count != MarkArrayTable.dbugGetAnchorCount())
            {
                throw new OpenFontNotSupportedException();
            }
            //--------------------------
            var expandedBase = new List<ushort>(BaseCoverageTable.GetExpandedValueIter());
            if (expandedBase.Count != BaseArrayTable.dbugGetRecordCount())
            {
                throw new OpenFontNotSupportedException();
            }
        }

#endif
    }
}