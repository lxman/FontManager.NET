using System.Collections.Generic;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// IndexSubTable5: constant - metrics glyphs with sparse glyph codes
    /// </summary>
    public class IndexSubTable5 : IndexSubTableBase
    {
        public override int SubTypeNo => 5;
        public uint imageSize;
        public BigGlyphMetrics BigGlyphMetrics = new BigGlyphMetrics();

        public ushort[] glyphIdArray;

        public override void BuildGlyphList(List<Glyph> glyphList)
        {
            uint incrementalOffset = 0;//TODO: review this
            for (int i = 0; i < glyphIdArray.Length; ++i)
            {
                glyphList.Add(new Glyph(glyphIdArray[i], header.imageDataOffset + incrementalOffset, imageSize, header.imageFormat));
                incrementalOffset += imageSize;
            }
        }
    }
}
