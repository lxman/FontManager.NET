using System;
using System.IO;

//from
//https://docs.microsoft.com/en-us/typography/opentype/spec/eblc
//https://docs.microsoft.com/en-us/typography/opentype/spec/ebdt
//https://docs.microsoft.com/en-us/typography/opentype/spec/cblc
//https://docs.microsoft.com/en-us/typography/opentype/spec/cbdt

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    /// <summary>
    /// Format 1: small metrics, byte-aligned data
    /// </summary>
    public class GlyphBitmapDataFmt1 : GlyphBitmapDataFormatBase
    {
        public override int FormatNumber => 1;
        public SmallGlyphMetrics smallGlyphMetrics;
        //Glyph bitmap format 1 consists of small metrics records(either horizontal or vertical
        //depending on the flags field of the BitmapSize table within the EBLC table)
        //followed by byte aligned bitmap data.

        //The bitmap data begins with the most significant bit of the
        //first byte corresponding to the top-left pixel of the bounding box,
        //proceeding through succeeding bits moving left to right.
        //The data for each row is padded to a byte boundary,
        //so the next row begins with the most significant bit of a new byte.

        //1 bits correspond to black, and 0 bits to white.

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
