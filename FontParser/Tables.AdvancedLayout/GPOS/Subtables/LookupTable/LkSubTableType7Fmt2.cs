using System;
using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class LkSubTableType7Fmt2 : LookupSubTable
    {
        public ClassDefTable.ClassDefTable ClassDef { get; set; }
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public PosClassSetTable[] PosClassSetTables { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            int lim = Math.Min(startAt + len, inputGlyphs.Count);
            for (int i = startAt; i < lim; ++i)
            {
                ushort glyph1Index = inputGlyphs.GetGlyph(i, out short unused);
                if (CoverageTable.FindPosition(glyph1Index) < 0)
                {
                    continue;
                }

                int glyph1Class = ClassDef.GetClassValue(glyph1Index);
                if (glyph1Class >= PosClassSetTables.Length || PosClassSetTables[glyph1Class] == null)
                {
                    continue;
                }

                foreach (PosClassRule rule in PosClassSetTables[glyph1Class].PosClassRules)
                {
                    ushort[] glyphIds = rule.InputGlyphIds;
                    var matches = 0;
                    for (var n = 0; n < glyphIds.Length && i + 1 + n < lim; ++n)
                    {
                        ushort glyphIndex = inputGlyphs.GetGlyph(i + 1 + n, out unused);
                        int glyphClass = ClassDef.GetClassValue(glyphIndex);
                        if (glyphClass != glyphIds[n])
                        {
                            break;
                        }
                        ++matches;
                    }

                    if (matches == glyphIds.Length)
                    {
                        foreach (PosLookupRecord plr in rule.PosLookupRecords)
                        {
                            LookupTable lookup = OwnerGPos.LookupList[plr.lookupListIndex];
                            lookup.DoGlyphPosition(inputGlyphs, i + plr.seqIndex, glyphIds.Length - plr.seqIndex);
                        }
                        break;
                    }
                }
            }
        }
    }
}