using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    internal class LkSubTableT6Fmt3 : LookupSubTable
    {
        public CoverageTable.CoverageTable[] BacktrackingCoverages { get; set; }
        public CoverageTable.CoverageTable[] InputCoverages { get; set; }
        public CoverageTable.CoverageTable[] LookaheadCoverages { get; set; }
        public SubstLookupRecord[] SubstLookupRecords { get; set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int inputLength = InputCoverages.Length;

            // Check that there are enough context glyphs
            if (pos < BacktrackingCoverages.Length ||
                inputLength + LookaheadCoverages.Length > len)
            {
                return false;
            }

            // Check all coverages: if any of them does not match, abort substitution
            for (var i = 0; i < InputCoverages.Length; ++i)
            {
                if (InputCoverages[i].FindPosition(glyphIndices[pos + i]) < 0)
                {
                    return false;
                }
            }

            for (var i = 0; i < BacktrackingCoverages.Length; ++i)
            {
                if (BacktrackingCoverages[i].FindPosition(glyphIndices[pos - 1 - i]) < 0)
                {
                    return false;
                }
            }

            for (var i = 0; i < LookaheadCoverages.Length; ++i)
            {
                if (LookaheadCoverages[i].FindPosition(glyphIndices[pos + inputLength + i]) < 0)
                {
                    return false;
                }
            }

            // It's a match! Perform substitutions and return true if anything changed
            if (SubstLookupRecords.Length == 0)
            {
                //handled,  NO substitution
                return true;
            }

            var hasChanged = false;
            foreach (SubstLookupRecord lookupRecord in SubstLookupRecords)
            {
                ushort replaceAt = lookupRecord.sequenceIndex;
                ushort lookupIndex = lookupRecord.lookupListIndex;

                LookupTable anotherLookup = OwnerGSub.LookupList[lookupIndex];
                if (anotherLookup.DoSubstitutionAt(glyphIndices, pos + replaceAt, len - replaceAt))
                {
                    hasChanged = true;
                }
            }

            return hasChanged;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            foreach (SubstLookupRecord lookupRecord in SubstLookupRecords)
            {
                ushort replaceAt = lookupRecord.sequenceIndex;
                ushort lookupIndex = lookupRecord.lookupListIndex;

                LookupTable anotherLookup = OwnerGSub.LookupList[lookupIndex];
                anotherLookup.CollectAssociatedSubstitutionGlyph(outputAssocGlyphs);
            }
        }
    }
}