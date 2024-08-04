using System;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    //format 3 Obsolete
    //format 4: not support in OpenFont

    //Format 5: metrics in EBLC, bit-aligned image data only
    public class GlyphBitmapDataFmt5 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 5;

        //Glyph bitmap format 5 is similar to format 2 except
        //that no metrics information is included, just the bit aligned data.
        //This format is for use with EBLC indexSubTable format 2 or format 5,
        //which will contain the metrics information for all glyphs. It works well for Kanji fonts.

        //The rasterizer recalculates
        //sbit metrics for Format 5 bitmap data,
        //allowing Windows to report correct ABC widths,
        //even if the bitmaps have white space on either side of the bitmap image.
        //This allows fonts to store monospaced bitmap glyphs in the efficient Format 5
        //without breaking Windows GetABCWidths call.
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
