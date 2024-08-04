using System.Collections.Generic;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    ///  IndexSubTable2: all glyphs have identical metrics
    /// </summary>
    public class IndexSubTable2 : IndexSubTableBase
    {
        public override int SubTypeNo => 2;
        public uint imageSize;
        public BigGlyphMetrics BigGlyphMetrics = new BigGlyphMetrics();

        public override void BuildGlyphList(List<Glyph> glyphList)
        {
            uint incrementalOffset = 0;//TODO: review this
            for (ushort n = firstGlyphIndex; n <= lastGlyphIndex; ++n)
            {
                glyphList.Add(new Glyph(n, header.imageDataOffset + incrementalOffset, imageSize, header.imageFormat));
                incrementalOffset += imageSize;
            }
        }
    }
}
