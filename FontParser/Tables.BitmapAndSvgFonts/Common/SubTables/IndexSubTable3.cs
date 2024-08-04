using System.Collections.Generic;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// IndexSubTable3: variable - metrics glyphs with 2 - byte offsets
    /// </summary>
    public class IndexSubTable3 : IndexSubTableBase
    {
        public override int SubTypeNo => 3;
        public ushort[] offsetArray;

        public override void BuildGlyphList(List<Glyph> glyphList)
        {
            int n = 0;
            for (ushort i = firstGlyphIndex; i <= lastGlyphIndex; ++i)
            {
                glyphList.Add(new Glyph(i, header.imageDataOffset + offsetArray[n++], 0, header.imageFormat));
            }
        }
    }
}
