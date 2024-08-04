using System;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// Format 6: big metrics, byte-aligned data
    /// </summary>
    public class GlyphBitmapDataFmt6 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 6;
        public BigGlyphMetrics bigMetrics;

        //Format 6: big metrics, byte-aligned data
        //Type            Name                  Description
        //BigGlyphMetrics bigMetrics            Metrics information for the glyph
        //uint8           imageData[variable]   Byte-aligned bitmap data

        //Glyph bitmap format 6 is the same as format 1 except that is uses big glyph metrics instead of small.
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
