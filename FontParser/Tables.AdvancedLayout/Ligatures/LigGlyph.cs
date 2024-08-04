using System.IO;

namespace FontParser.Tables.AdvancedLayout.Ligatures
{
    //A Ligature Glyph table (LigGlyph) contains the caret coordinates for a single ligature glyph.
    //The number of coordinate values, each defined in a separate CaretValue table,
    //equals the number of components in the ligature minus one (1).***

    //The LigGlyph table consists of a count of the number of CaretValue tables defined for the ligature (CaretCount) and
    //an array of offsets to CaretValue tables (CaretValue).

    //Example 4 at the end of the chapter shows a LigGlyph table.
    //LigGlyph table
    //Type  	Name 	                    Description
    //uint16 	CaretCount 	                Number of CaretValues for this ligature (components - 1)
    //Offset16 	CaretValue[CaretCount] 	    Array of offsets to CaretValue tables-from beginning of LigGlyph table-in increasing coordinate order Caret Values Table

    /// <summary>
    /// A Ligature Glyph table (LigGlyph) contains the caret coordinates for a single ligature glyph.
    /// </summary>
    public class LigGlyph
    {
        private ushort[] _caretValueOffsets;

        public static LigGlyph CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //----------
            LigGlyph ligGlyph = new LigGlyph();
            ushort caretCount = reader.ReadUInt16();
            ligGlyph._caretValueOffsets = reader.ReadUInt16Array(caretCount);
            return ligGlyph;
        }
    }
}
