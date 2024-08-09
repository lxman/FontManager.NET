using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class PosRuleSetTable
    {
        //PosRuleSet table: All contexts beginning with the same glyph
        // Value 	Type 	        Description
        //uint16 	PosRuleCount 	Number of PosRule tables
        //Offset16 	PosRule[PosRuleCount] 	Array of offsets to PosRule tables-from beginning of PosRuleSet-ordered by preference
        //
        //A PosRule table consists of a count of the glyphs to be matched in the input context sequence (GlyphCount),
        //including the first glyph in the sequence, and an array of glyph indices that describe the context (Input).
        //The Coverage table specifies the index of the first glyph in the context, and the Input array begins with the second glyph in the context sequence. As a result, the first index position in the array is specified with the number one (1), not zero (0). The Input array lists the indices in the order the corresponding glyphs appear in the text. For text written from right to left, the right-most glyph will be first; conversely, for text written from left to right, the left-most glyph will be first.

        //A PosRule table also contains a count of the positioning operations to be performed on the input glyph sequence (PosCount) and an array of PosLookupRecords (PosLookupRecord). Each record specifies a position in the input glyph sequence and a LookupList index to the positioning lookup to be applied there. The array should list records in design order, or the order the lookups should be applied to the entire glyph sequence.

        //Example 10 at the end of this chapter demonstrates glyph kerning in context with a ContextPosFormat1 subtable.

        private PosRuleTable[] _posRuleTables;

        private void ReadFrom(BinaryReader reader)
        {
            long tableStartAt = reader.BaseStream.Position;
            ushort posRuleCount = reader.ReadUInt16();
            ushort[] posRuleTableOffsets = reader.ReadUInt16Array(posRuleCount);
            int j = posRuleTableOffsets.Length;
            _posRuleTables = new PosRuleTable[posRuleCount];
            for (var i = 0; i < j; ++i)
            {
                //move to and read
                reader.BaseStream.Seek(tableStartAt + posRuleTableOffsets[i], SeekOrigin.Begin);
                var posRuleTable = new PosRuleTable();
                posRuleTable.ReadFrom(reader);
                _posRuleTables[i] = posRuleTable;
            }
        }

        public static PosRuleSetTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //------------
            var posRuleSetTable = new PosRuleSetTable();
            posRuleSetTable.ReadFrom(reader);
            return posRuleSetTable;
        }
    }
}