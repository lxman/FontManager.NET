using System.Collections.Generic;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// IndexSubTable1: variable - metrics glyphs with 4 - byte offsets
    /// </summary>
    public class IndexSubTable1 : IndexSubTableBase
    {
        public override int SubTypeNo => 1;
        public uint[] offsetArray;

        public override void BuildGlyphList(List<Glyph> glyphList)
        {
            int n = 0;
            for (ushort i = firstGlyphIndex; i <= lastGlyphIndex; ++i)
            {
                glyphList.Add(new Glyph(i, header.imageDataOffset + offsetArray[n], 0, header.imageFormat));
                n++;
            }
        }
    }
}
