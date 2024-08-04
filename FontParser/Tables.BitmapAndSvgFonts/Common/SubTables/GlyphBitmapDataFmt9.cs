using System;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    //Glyph bitmap formats 8 and 9 are used for composite bitmaps.
    //For accented characters and other composite glyphs
    //it may be more efficient to store
    //a copy of each component separately,
    //and then use a composite description to construct the finished glyph.

    //The composite formats allow for any number of components,
    //and allow the components to be positioned anywhere in the finished glyph.
    //Format 8 uses small metrics, and format 9 uses big metrics.

    /// <summary>
    /// Format 9:
    /// </summary>
    public class GlyphBitmapDataFmt9 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 9;
        public BigGlyphMetrics bigMetrics;
        public EbdtComponent[] components;

        //Format 9: big metrics, component data
        //Type              Name            Description
        //BigGlyphMetrics   bigMetrics      Metrics information for the glyph
        //uint16            numComponents   Number of components
        //EbdtComponent     components[numComponents] Array of EbdtComponent records
        public override void FillGlyphInfo(BinaryReader reader, Glyph bitmapGlyph)
        {
            throw new NotImplementedException();
        }

        public override void ReadRawBitmap(BinaryReader reader, Glyph bitmapGlyph, Stream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
