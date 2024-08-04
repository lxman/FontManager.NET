using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class ChainSubRuleSetTable
    {
        //ChainSubRuleSet table: All contexts beginning with the same glyph
        //-------------------------------------------------------------------------
        //Type  	Name 	                            Description
        //-------------------------------------------------------------------------
        //uint16 	ChainSubRuleCount 	                Number of ChainSubRule tables
        //Offset16 	ChainSubRule[ChainSubRuleCount] 	Array of offsets to ChainSubRule tables-from beginning of ChainSubRuleSet table-ordered by preference
        //-------------------------------------------------------------------------
        //
        //A ChainSubRule table consists of a count of the glyphs to be matched in the backtrack,
        //input, and lookahead context sequences, including the first glyph in each sequence,
        //and an array of glyph indices that describe each portion of the contexts.
        //The Coverage table specifies the index of the first glyph in each context,
        //and each array begins with the second glyph (array index = 1) in the context sequence.

        // Note: All arrays list the indices in the order the corresponding glyphs appear in the text.
        //For text written from right to left, the right-most glyph will be first; conversely,
        //for text written from left to right, the left-most glyph will be first.

        private ChainSubRuleSubTable[] _chainSubRuleSubTables;

        public static ChainSubRuleSetTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---
            ChainSubRuleSetTable table = new ChainSubRuleSetTable();
            ushort subRuleCount = reader.ReadUInt16();
            ushort[] subRuleOffsets = reader.ReadUInt16Array(subRuleCount);
            ChainSubRuleSubTable[] chainSubRuleSubTables = table._chainSubRuleSubTables = new ChainSubRuleSubTable[subRuleCount];
            for (int i = 0; i < subRuleCount; ++i)
            {
                chainSubRuleSubTables[i] = ChainSubRuleSubTable.CreateFrom(reader, beginAt + subRuleOffsets[i]);
            }

            return table;
        }
    }
}