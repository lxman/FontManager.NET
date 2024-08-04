using System;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// Format 2: small metrics, bit-aligned data
    /// </summary>
    public class GlyphBitmapDataFmt2 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 2;

        //Glyph bitmap format 2 is the same as format 1 except
        //that the bitmap data is bit aligned.

        //This means that the data for a new row will begin with the bit immediately
        //following the last bit of the previous row.
        //The start of each glyph must be byte aligned,
        //so the last row of a glyph may require padding.

        //This format takes a little more time to parse, but saves file space compared to format 1.
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
