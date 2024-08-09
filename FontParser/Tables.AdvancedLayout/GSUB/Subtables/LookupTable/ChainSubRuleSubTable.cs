using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    //---------------------
    //SubstLookupRecord
    //---------------------
    //Type 	    Name 	            Description
    //uint16 	SequenceIndex 	    Index into current glyph sequence-first glyph = 0
    //uint16 	LookupListIndex 	Lookup to apply to that position-zero-based
    //---------------------
    //The SequenceIndex in a SubstLookupRecord must take into consideration the order
    //in which lookups are applied to the entire glyph sequence.
    //Because multiple substitutions may occur per context,
    //the SequenceIndex and LookupListIndex refer to the glyph sequence after the text-processing client has applied any previous lookups.
    //In other words, the SequenceIndex identifies the location for the substitution at the time that the lookup is to be applied.
    //For example, consider an input glyph sequence of four glyphs.
    //The first glyph does not have a substitute, but the middle
    //two glyphs will be replaced with a ligature, and a single glyph will replace the fourth glyph:

    //    The first glyph is in position 0. No lookups will be applied at position 0, so no SubstLookupRecord is defined.
    //    The SubstLookupRecord defined for the ligature substitution specifies the SequenceIndex as position 1,
    //which is the position of the first-glyph component in the ligature string. After the ligature replaces the glyphs in positions 1 and 2, however,
    //the input glyph sequence consists of only three glyphs, not the original four.
    //    To replace the last glyph in the sequence,
    //the SubstLookupRecord defines the SequenceIndex as position 2 instead of position 3.
    //This position reflects the effect of the ligature substitution applied before this single substitution.

    //    Note: This example assumes that the LookupList specifies the ligature substitution lookup before the single substitution lookup.
    internal class ChainSubRuleSubTable
    {
        //A ChainSubRule table also contains a count of the substitutions to be performed on the input glyph sequence (SubstCount)
        //and an array of SubstitutionLookupRecords (SubstLookupRecord).
        //Each record specifies a position in the input glyph sequence and a LookupListIndex to the substitution lookup that is applied at that position.
        //The array should list records in design order, or the order the lookups should be applied to the entire glyph sequence.

        //ChainSubRule subtable
        //Type 	    Name 	                            Description
        //uint16 	BacktrackGlyphCount 	            Total number of glyphs in the backtrack sequence (number of glyphs to be matched before the first glyph)
        //uint16 	Backtrack[BacktrackGlyphCount] 	    Array of backtracking GlyphID's (to be matched before the input sequence)
        //uint16 	InputGlyphCount 	                Total number of glyphs in the input sequence (includes the first glyph)
        //uint16 	Input[InputGlyphCount - 1] 	        Array of input GlyphIDs (start with second glyph)
        //uint16 	LookaheadGlyphCount 	            Total number of glyphs in the look ahead sequence (number of glyphs to be matched after the input sequence)
        //uint16 	LookAhead[LookAheadGlyphCount]  	Array of lookahead GlyphID's (to be matched after the input sequence)
        //uint16 	SubstCount 	                        Number of SubstLookupRecords
        //struct 	SubstLookupRecord[SubstCount] 	    Array of SubstLookupRecords (in design order)

        private ushort[] backTrackingGlyphs;
        private ushort[] inputGlyphs;
        private ushort[] lookaheadGlyphs;
        private SubstLookupRecord[] substLookupRecords;

        public static ChainSubRuleSubTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            //------------
            var subRuleTable = new ChainSubRuleSubTable();
            ushort backtrackGlyphCount = reader.ReadUInt16();
            subRuleTable.backTrackingGlyphs = reader.ReadUInt16Array(backtrackGlyphCount);
            //--------
            ushort inputGlyphCount = reader.ReadUInt16();
            subRuleTable.inputGlyphs = reader.ReadUInt16Array(inputGlyphCount - 1);//*** start with second glyph, so -1
                                                                                   //----------
            ushort lookaheadGlyphCount = reader.ReadUInt16();
            subRuleTable.lookaheadGlyphs = reader.ReadUInt16Array(lookaheadGlyphCount);
            //------------
            ushort substCount = reader.ReadUInt16();
            subRuleTable.substLookupRecords = SubstLookupRecord.CreateSubstLookupRecords(reader, substCount);

            return subRuleTable;
        }
    }
}