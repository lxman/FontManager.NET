using System.IO;

namespace FontParser.Tables.AdvancedLayout.Ligatures
{
    //from https://docs.microsoft.com/en-us/typography/opentype/spec/gdef
    //Ligature Caret List Table
    //The Ligature Caret List table (LigCaretList) defines caret positions for all the ligatures in a font.
    //The table consists of an offset to a Coverage table that lists all the ligature glyphs (Coverage),
    //a count of the defined ligatures (LigGlyphCount),
    //and an array of offsets to LigGlyph tables (LigGlyph).

    //The array lists the LigGlyph tables,
    //one for each ligature in the Coverage table, in the same order as the Coverage Index.

    //Example 4 at the end of this chapter shows a LigCaretList table.
    //LigCaretList table
    //Type 	    Name 	        Description
    //Offset16 	Coverage 	    Offset to Coverage table - from beginning of LigCaretList table
    //uint16 	LigGlyphCount 	Number of ligature glyphs
    //Offset16 	LigGlyph[LigGlyphCount] 	Array of offsets to LigGlyph tables-from beginning of LigCaretList table-in Coverage Index order

    /// <summary>
    /// Ligature Caret List Table, defines caret positions for all the ligatures in a font
    /// </summary>
    public class LigCaretList
    {
        private LigGlyph[] _ligGlyphs;
        private CoverageTable.CoverageTable _coverageTable;

        public static LigCaretList CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //----
            var ligCaretList = new LigCaretList();
            ushort coverageOffset = reader.ReadUInt16();
            ushort ligGlyphCount = reader.ReadUInt16();
            ushort[] ligGlyphOffsets = reader.ReadUInt16Array(ligGlyphCount);
            LigGlyph[] ligGlyphs = new LigGlyph[ligGlyphCount];
            for (var i = 0; i < ligGlyphCount; ++i)
            {
                ligGlyphs[i] = LigGlyph.CreateFrom(reader, beginAt + ligGlyphOffsets[i]);
            }
            ligCaretList._ligGlyphs = ligGlyphs;
            ligCaretList._coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, beginAt + coverageOffset);
            return ligCaretList;
        }
    }
}
