using System;
using System.Collections.Generic;
using System.IO;

namespace FontParser.Tables.AdvancedLayout.CoverageTable
{
    public class CoverageFmt1 : CoverageTable
    {
        public static CoverageFmt1 CreateFrom(BinaryReader reader)
        {
            // CoverageFormat1 table: Individual glyph indices
            // Type      Name                     Description
            // uint16    CoverageFormat           Format identifier-format = 1
            // uint16    GlyphCount               Number of glyphs in the GlyphArray
            // uint16    GlyphArray[GlyphCount]   Array of glyph IDs — in numerical order

            ushort glyphCount = reader.ReadUInt16();
            ushort[] glyphs = reader.ReadUInt16Array(glyphCount);
            return new CoverageFmt1 { _orderedGlyphIdList = glyphs };
        }

        public override int FindPosition(ushort glyphIndex)
        {
            // "The glyph indices must be in numerical order for binary searching of the list"
            // (https://www.microsoft.com/typography/otspec/chapter2.htm#coverageFormat1)
            int n = Array.BinarySearch(_orderedGlyphIdList, glyphIndex);
            return n < 0 ? -1 : n;
        }

        public override IEnumerable<ushort> GetExpandedValueIter()
        { return _orderedGlyphIdList; }

#if DEBUG

        public override string ToString()
        {
            List<string> stringList = new List<string>();
            foreach (ushort g in _orderedGlyphIdList)
            {
                stringList.Add(g.ToString());
            }
            return "CoverageFmt1: " + string.Join(",", stringList.ToArray());
        }

#endif

        internal ushort[] _orderedGlyphIdList;
    }
}
