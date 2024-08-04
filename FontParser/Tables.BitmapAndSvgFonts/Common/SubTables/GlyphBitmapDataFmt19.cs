using System;
using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    public class GlyphBitmapDataFmt19 : GlyphBitmapDataFormatBase
    {
        //Format 19: metrics in CBLC table, PNG image data
        //Type    Name          Description
        //uint32  dataLen       Length of data in bytes
        //uint8   data[dataLen] Raw PNG data
        public override int FormatNumber => 19;

        public override void FillGlyphInfo(BinaryReader reader, Glyph bitmapGlyph)
        {
            //no glyph info to fill
            //TODO::....
        }

        public override void ReadRawBitmap(BinaryReader reader, Glyph bitmapGlyph, Stream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
