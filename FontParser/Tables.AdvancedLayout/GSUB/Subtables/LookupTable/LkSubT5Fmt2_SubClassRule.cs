using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LkSubT5Fmt2_SubClassRule
    {
        //SubClassRule table: Context definition for one class
        //Table 19
        //Type 	    Name 	            Description
        //uint16 	glyphCount 	        Total number of classes specified for the context in the rule — includes the first class
        //uint16 	substitutionCount 	Number of SubstLookupRecords
        //uint16 	inputSequence[glyphCount - 1] 	Array of classes to be matched to the input glyph sequence, beginning with the second glyph position.
        //SubstLookupRecord 	        substLookupRecords[substitutionCount] 	Array of Substitution lookups, in design order.

        public ushort[] inputSequence;
        public SubstLookupRecord[] substRecords;

        public void ReadFrom(BinaryReader reader, long pos)
        {
            reader.BaseStream.Seek(pos, SeekOrigin.Begin);

            ushort glyphCount = reader.ReadUInt16();
            ushort substitutionCount = reader.ReadUInt16();
            inputSequence = reader.ReadUInt16Array(glyphCount - 1);
            substRecords = SubstLookupRecord.CreateSubstLookupRecords(reader, substitutionCount);
        }
    }
}