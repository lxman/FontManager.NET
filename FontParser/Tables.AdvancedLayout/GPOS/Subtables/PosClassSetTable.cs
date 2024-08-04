using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class PosClassSetTable
    {
        //PosClassSet table: All contexts beginning with the same class
        //Value 	Type 	                        Description
        //----------------------
        //uint16 	PosClassRuleCnt 	            Number of PosClassRule tables
        //Offset16 	PosClassRule[PosClassRuleCnt] 	Array of offsets to PosClassRule tables-from beginning of PosClassSet-ordered by preference
        //----------------------
        //
        //For each context, a PosClassRule table contains a count of the glyph classes in a given context (GlyphCount),
        //including the first class in the context sequence.
        //A class array lists the classes, beginning with the second class,
        //that follow the first class in the context.
        //The first class listed indicates the second position in the context sequence.

        //Note: Text order depends on the writing direction of the text.
        //For text written from right to left, the right-most glyph will be first.
        //Conversely, for text written from left to right, the left-most glyph will be first.

        //The values specified in the Class array are those defined in the ClassDef table.
        //For example, consider a context consisting of the sequence: Class 2, Class 7, Class 5, Class 0.
        //The Class array will read: Class[0] = 7, Class[1] = 5, and Class[2] = 0.
        //The first class in the sequence, Class 2, is defined by the index into the PosClassSet array of offsets.
        //The total number and sequence of glyph classes listed in the Class array must match the total number and sequence of glyph classes contained in the input context.

        //A PosClassRule also contains a count of the positioning operations to be performed on the context (PosCount) and
        //an array of PosLookupRecords (PosLookupRecord) that supply the positioning data.
        //For each position in the context that requires a positioning operation,
        //a PosLookupRecord specifies a LookupList index and a position in the input glyph class sequence where the lookup is applied.
        //The PosLookupRecord array lists PosLookupRecords in design order, or the order in which lookups are applied to the entire glyph sequence.

        //Example 11 at the end of this chapter demonstrates a ContextPosFormat2 subtable that uses glyph classes to modify accent positions in glyph strings.
        //----------------------
        //PosClassRule table: One class context definition
        //----------------------
        //Value 	Type 	    Description
        //uint16 	GlyphCount 	Number of glyphs to be matched
        //uint16 	PosCount 	Number of PosLookupRecords
        //uint16 	Class[GlyphCount - 1] 	Array of classes-beginning with the second class-to be matched to the input glyph sequence
        //struct 	PosLookupRecord[PosCount] 	Array of positioning lookups-in design order
        //----------------------

        public PosClassRule[] PosClassRules;

        private void ReadFrom(BinaryReader reader)
        {
            long tableStartAt = reader.BaseStream.Position;
            //
            ushort posClassRuleCnt = reader.ReadUInt16();
            ushort[] posClassRuleOffsets = reader.ReadUInt16Array(posClassRuleCnt);
            PosClassRules = new PosClassRule[posClassRuleCnt];
            for (int i = 0; i < posClassRuleOffsets.Length; ++i)
            {
                //move to and read
                PosClassRules[i] = PosClassRule.CreateFrom(reader, tableStartAt + posClassRuleOffsets[i]);
            }
        }

        public static PosClassSetTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //--------
            var posClassSetTable = new PosClassSetTable();
            posClassSetTable.ReadFrom(reader);
            return posClassSetTable;
        }
    }
}