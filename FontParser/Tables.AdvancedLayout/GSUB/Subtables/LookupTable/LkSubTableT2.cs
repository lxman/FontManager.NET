using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LkSubTableT2 : LookupSubTable
    {
        public CoverageTable.CoverageTable CoverageTable { get; set; }
        public SequenceTable[] SeqTables { get; set; }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            int foundPos = CoverageTable.FindPosition(glyphIndices[pos]);
            if (foundPos > -1)
            {
                SequenceTable seqTable = SeqTables[foundPos];
                //replace current glyph index with new seq
#if DEBUG
                int new_seqCount = seqTable.substituteGlyphs.Length;
#endif
                glyphIndices.Replace(pos, seqTable.substituteGlyphs);
                return true;
            }
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            foreach (ushort glyphIndex in CoverageTable.GetExpandedValueIter())
            {
                int pos = CoverageTable.FindPosition(glyphIndex);
#if DEBUG
                if (pos >= SeqTables.Length)
                {
                }
#endif
                outputAssocGlyphs.AddRange(SeqTables[pos].substituteGlyphs);
            }
        }
    }
}