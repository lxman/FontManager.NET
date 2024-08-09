using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// 5.2 Context Substitution Format 2: Class-based Glyph Contexts
    /// </summary>
    public class LkSubTableT5Fmt2 : LookupSubTable
    {
        //Format 2, a more flexible format than Format 1,
        //describes class-based context substitution.
        //For this format, a specific integer, called a class value, must be assigned to each glyph component in all context glyph sequences.
        //Contexts are then defined as sequences of glyph class values.
        //More than one context may be defined at a time.

        public CoverageTable.CoverageTable coverageTable;
        public ClassDefTable.ClassDefTable classDef;
        public LkSubT5Fmt2_SubClassSet[] subClassSets;

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            //collect only assoc
            var collected = new Dictionary<int, bool>();
            foreach (ushort glyphIndex in coverageTable.GetExpandedValueIter())
            {
                int class_value = classDef.GetClassValue(glyphIndex);
                if (!collected.TryAdd(class_value, true))
                {
                    continue;
                }
                //

                LkSubT5Fmt2_SubClassSet subClassSet = subClassSets[class_value];
                LkSubT5Fmt2_SubClassRule[] subClassRules = subClassSet.subClassRules;

                for (var i = 0; i < subClassRules.Length; ++i)
                {
                    LkSubT5Fmt2_SubClassRule rule = subClassRules[i];
                    if (rule != null && rule.substRecords != null)
                    {
                        for (var n = 0; n < rule.substRecords.Length; ++n)
                        {
                            SubstLookupRecord rect = rule.substRecords[n];
                            LookupTable anotherLookup = OwnerGSub.LookupList[rect.lookupListIndex];
                            anotherLookup.CollectAssociatedSubstitutionGlyph(outputAssocGlyphs);
                        }
                    }
                }
            }
        }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int coverage_pos = coverageTable.FindPosition(glyphIndices[pos]);
            if (coverage_pos < 0) { return false; }

            int class_value = classDef.GetClassValue(glyphIndices[pos]);

            LkSubT5Fmt2_SubClassSet subClassSet = subClassSets[class_value];
            LkSubT5Fmt2_SubClassRule[] subClassRules = subClassSet.subClassRules;

            for (var i = 0; i < subClassRules.Length; ++i)
            {
                LkSubT5Fmt2_SubClassRule rule = subClassRules[i];
                ushort[] inputSequence = rule.inputSequence; //clas seq
                int next_pos = pos + 1;

                if (next_pos < glyphIndices.Count)
                {
                    var passAll = true;
                    for (var a = 0; a < inputSequence.Length && next_pos < glyphIndices.Count; ++a, ++next_pos)
                    {
                        int class_next = glyphIndices[next_pos];
                        if (inputSequence[a] != class_next)
                        {
                            passAll = false;
                            break;
                        }
                    }
                    if (passAll)
                    {
                    }
                }
            }

            Utils.WarnUnimplemented("GSUB,unfinish:" + nameof(LkSubTableT5Fmt2));
            return false;
        }
    }
}