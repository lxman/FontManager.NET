using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LkSubT5Fmt1_SubRule
    {
        //SubRule table: One simple context definition
        //Table 16
        //Type 	            Name 	                            Description
        //uint16 	        glyphCount 	                        Total number of glyphs in input glyph sequence — includes the first glyph.
        //uint16 	        substitutionCount 	                Number of SubstLookupRecords
        //uint16 	        inputSequence[glyphCount - 1] 	    Array of input glyph IDs — start with second glyph
        //SubstLookupRecord substLookupRecords[substitutionCount] 	Array of SubstLookupRecords, in design order

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