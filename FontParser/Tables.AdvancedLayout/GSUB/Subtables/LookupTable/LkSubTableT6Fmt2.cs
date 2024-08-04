using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    internal class LkSubTableT6Fmt2 : LookupSubTable
    {
        //Format 2 describes class-based chaining context substitution.
        //For this format, a specific integer, called a class value,
        //must be assigned to each glyph component in all context glyph sequences.
        //Contexts are then defined as sequences of glyph class values.
        //More than one context may be defined at a time.

        //For this format, the Coverage table lists indices for the complete set of unique glyphs
        //(not glyph classes) that may appear as the first glyph of any class-based context.
        //In other words, the Coverage table contains the list of glyph indices for all the glyphs in all classes
        //that may be first in any of the context class sequences

        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public ClassDefTable.ClassDefTable BacktrackClassDef { get; set; }
        public ClassDefTable.ClassDefTable InputClassDef { get; set; }
        public ClassDefTable.ClassDefTable LookaheadClassDef { get; set; }
        public ChainSubClassSet[] ChainSubClassSets { get; set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int coverage_pos = CoverageTable.FindPosition(glyphIndices[pos]);
            if (coverage_pos < 0) { return false; }

            //--

            int inputClass = InputClassDef.GetClassValue(glyphIndices[pos]);

            Utils.WarnUnimplemented("GSUB, " + nameof(LkSubTableT6Fmt2));
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            Utils.WarnUnimplementedCollectAssocGlyphs(ToString());
        }
    }
}