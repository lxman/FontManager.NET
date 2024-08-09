using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class PairSetTable
    {
        internal PairSet[] _pairSets;

        public void ReadFrom(BinaryReader reader, ushort v1format, ushort v2format)
        {
            ushort rowCount = reader.ReadUInt16();
            _pairSets = new PairSet[rowCount];
            for (var i = 0; i < rowCount; ++i)
            {
                //GlyphID 	    SecondGlyph 	GlyphID of second glyph in the pair-first glyph is listed in the Coverage table
                //ValueRecord 	Value1 	        Positioning data for the first glyph in the pair
                //ValueRecord 	Value2 	        Positioning data for the second glyph in the pair
                ushort secondGlyph = reader.ReadUInt16();
                var v1 = ValueRecord.CreateFrom(reader, v1format);
                var v2 = ValueRecord.CreateFrom(reader, v2format);
                //
                _pairSets[i] = new PairSet(secondGlyph, v1, v2);
            }
        }

        public bool FindPairSet(ushort secondGlyphIndex, out PairSet foundPairSet)
        {
            int j = _pairSets.Length;
            for (var i = 0; i < j; ++i)
            {
                //TODO: binary search?
                if (_pairSets[i].secondGlyph == secondGlyphIndex)
                {
                    //found
                    foundPairSet = _pairSets[i];
                    return true;
                }
            }
            //
            foundPairSet = new PairSet();//empty
            return false;
        }
    }
}