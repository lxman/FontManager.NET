using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    /// <summary>
    /// Lookup Type2, Format2: Class pair adjustment
    /// </summary>
    internal class LkSubTableType2Fmt2 : LookupSubTable
    {
        //Format 2 defines a pair as a set of two glyph classes and modifies the positions of all the glyphs in a class
        internal readonly Lk2Class1Record[] _class1records;

        internal readonly ClassDefTable.ClassDefTable _class1Def;
        internal readonly ClassDefTable.ClassDefTable _class2Def;

        public LkSubTableType2Fmt2(Lk2Class1Record[] class1records, ClassDefTable.ClassDefTable class1Def, ClassDefTable.ClassDefTable class2Def)
        {
            _class1records = class1records;
            _class1Def = class1Def;
            _class2Def = class2Def;
        }

        public CoverageTable.CoverageTable CoverageTable { get; set; }

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            //coverage
            //The Coverage table lists the indices of the first glyphs that may appear in each glyph pair.
            //More than one pair may begin with the same glyph,
            //but the Coverage table lists the glyph index only once

            CoverageTable.CoverageTable covTable = CoverageTable;
            int lim = inputGlyphs.Count - 1;
            for (int i = 0; i < lim; ++i) //start at 0
            {
                ushort glyph1_index = inputGlyphs.GetGlyph(i, out short glyph_advW);
                int record1Index = covTable.FindPosition(glyph1_index);
                if (record1Index > -1)
                {
                    int class1_no = _class1Def.GetClassValue(glyph1_index);
                    if (class1_no > -1)
                    {
                        ushort glyph2_index = inputGlyphs.GetGlyph(i + 1, out short glyph_advW2);
                        int class2_no = _class2Def.GetClassValue(glyph2_index);

                        if (class2_no > -1)
                        {
                            Lk2Class1Record class1Rec = _class1records[class1_no];
                            //TODO: recheck for vertical writing ... (YAdvance)
                            Lk2Class2Record pair = class1Rec.class2Records[class2_no];

                            ValueRecord v1 = pair.value1;
                            ValueRecord v2 = pair.value2;

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
}