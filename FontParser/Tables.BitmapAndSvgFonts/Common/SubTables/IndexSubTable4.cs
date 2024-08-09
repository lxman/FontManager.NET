using System.Collections.Generic;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// IndexSubTable4: variable - metrics glyphs with sparse glyph codes
    /// </summary>
    public class IndexSubTable4 : IndexSubTableBase
    {
        public override int SubTypeNo => 4;
        public GlyphIdOffsetPair[] glyphArray;

        public override void BuildGlyphList(List<Glyph> glyphList)
        {
            for (var i = 0; i < glyphArray.Length; ++i)
            {
                GlyphIdOffsetPair pair = glyphArray[i];
                glyphList.Add(new Glyph(pair.glyphId, header.imageDataOffset + pair.offset, 0, header.imageFormat));
            }
        }
    }
}
