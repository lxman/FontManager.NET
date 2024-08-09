using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class ChainSubClassSet
    {
        //----------------------------------
        //ChainSubRuleSet table: All contexts beginning with the same glyph
        //----------------------------------
        //Type 	    Name 	                Description
        //uint16 	ChainSubClassRuleCnt 	Number of ChainSubClassRule tables
        //Offset16 	ChainSubClassRule[ChainSubClassRuleCount] 	Array of offsets to ChainSubClassRule tables-from beginning of ChainSubClassSet-ordered by preference
        //----------------------------------
        //For each context, a ChainSubClassRule table contains a count of the glyph classes in the context sequence (GlyphCount),
        //including the first class.
        //A Class array lists the classes, beginning with the second class (array index = 1), that follow the first class in the context.

        //Note: Text order depends on the writing direction of the text. For text written from right to left, the right-most class will be first. Conversely, for text written from left to right, the left-most class will be first.

        //The values specified in the Class array are the values defined in the ClassDef table.
        //The first class in the sequence,
        //Class 2, is identified in the ChainContextSubstFormat2 table by the ChainSubClassSet array index of the corresponding ChainSubClassSet.

        //A ChainSubClassRule also contains a count of the substitutions to be performed on the context (SubstCount) and an array of SubstLookupRecords (SubstLookupRecord) that supply the substitution data. For each position in the context that requires a substitution, a SubstLookupRecord specifies a LookupList index and a position in the input glyph sequence where the lookup is applied. The SubstLookupRecord array lists SubstLookupRecords in design order-that is, the order in which lookups should be applied to the entire glyph sequence.

        private ChainSubClassRuleTable[] subClassRuleTables;

        public static ChainSubClassSet CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            var chainSubClassSet = new ChainSubClassSet();
            ushort count = reader.ReadUInt16();
            ushort[] subClassRuleOffsets = reader.ReadUInt16Array(count);

            ChainSubClassRuleTable[] subClassRuleTables = chainSubClassSet.subClassRuleTables = new ChainSubClassRuleTable[count];
            for (var i = 0; i < count; ++i)
            {
                subClassRuleTables[i] = ChainSubClassRuleTable.CreateFrom(reader, beginAt + subClassRuleOffsets[i]);
            }
            return chainSubClassSet;
        }
    }
}