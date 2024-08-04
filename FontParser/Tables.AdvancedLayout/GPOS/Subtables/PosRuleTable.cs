using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class PosRuleTable
    {
        //PosRule subtable
        //Value 	Type 	    Description
        //uint16 	GlyphCount 	Number of glyphs in the Input glyph sequence
        //uint16 	PosCount 	Number of PosLookupRecords
        //uint16 	Input[GlyphCount - 1]  Array of input GlyphIDs-starting with the second glyph***
        //struct 	PosLookupRecord[PosCount] 	Array of positioning lookups-in design order
        private PosLookupRecord[] _posLookupRecords;

        private ushort[] _inputGlyphIds;

        public void ReadFrom(BinaryReader reader)
        {
            ushort glyphCount = reader.ReadUInt16();
            ushort posCount = reader.ReadUInt16();
            _inputGlyphIds = reader.ReadUInt16Array(glyphCount - 1);
            _posLookupRecords = GPOS.CreateMultiplePosLookupRecords(reader, posCount);
        }
    }
}