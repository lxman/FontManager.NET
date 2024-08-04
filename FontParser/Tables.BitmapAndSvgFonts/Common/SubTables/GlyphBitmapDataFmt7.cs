using System;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// Format7: big metrics, bit-aligned data
    /// </summary>
    public class GlyphBitmapDataFmt7 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 7;

        public BigGlyphMetrics bigMetrics;

        //
        //Type                Name                  Description
        //BigGlyphMetrics     bigMetrics            Metrics information for the glyph
        //uint8               imageData[variable]   Bit-aligned bitmap data
        //Glyph bitmap format 7 is the same as format 2 except that is uses big glyph metrics instead of small.
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
