using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    internal class ChainSubClassRuleTable
    {
        //ChainSubClassRule table: Chaining context definition for one class
        //Type 	    Name 	                        Description
        //USHORT 	BacktrackGlyphCount 	        Total number of glyphs in the backtrack sequence (number of glyphs to be matched before the first glyph)
        //USHORT 	Backtrack[BacktrackGlyphCount] 	Array of backtracking classes(to be matched before the input sequence)
        //USHORT 	InputGlyphCount 	            Total number of classes in the input sequence (includes the first class)
        //USHORT 	Input[InputGlyphCount - 1] 	    Array of input classes(start with second class; to be matched with the input glyph sequence)
        //USHORT 	LookaheadGlyphCount 	        Total number of classes in the look ahead sequence (number of classes to be matched after the input sequence)
        //USHORT 	LookAhead[LookAheadGlyphCount] 	Array of lookahead classes(to be matched after the input sequence)
        //USHORT 	SubstCount 	                    Number of SubstLookupRecords
        //struct 	SubstLookupRecord[SubstCount] 	Array of SubstLookupRecords (in design order)

        private ushort[] backtrakcingClassDefs;
        private ushort[] inputClassDefs;
        private ushort[] lookaheadClassDefs;
        private SubstLookupRecord[] subsLookupRecords;

        public static ChainSubClassRuleTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            var subClassRuleTable = new ChainSubClassRuleTable();
            ushort backtrackingCount = reader.ReadUInt16();
            subClassRuleTable.backtrakcingClassDefs = reader.ReadUInt16Array(backtrackingCount);
            ushort inputGlyphCount = reader.ReadUInt16();
            subClassRuleTable.inputClassDefs = reader.ReadUInt16Array(inputGlyphCount - 1);//** -1
            ushort lookaheadGlyphCount = reader.ReadUInt16();
            subClassRuleTable.lookaheadClassDefs = reader.ReadUInt16Array(lookaheadGlyphCount);
            ushort substCount = reader.ReadUInt16();
            subClassRuleTable.subsLookupRecords = SubstLookupRecord.CreateSubstLookupRecords(reader, substCount);

            return subClassRuleTable;
        }
    }
}