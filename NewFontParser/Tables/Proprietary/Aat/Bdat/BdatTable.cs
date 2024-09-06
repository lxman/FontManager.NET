using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Proprietary.Aat.Bdat.GlyphBitmap;

namespace NewFontParser.Tables.Proprietary.Aat.Bdat
{
    public class BdatTable : IInfoTable
    {
        public static string Tag => "bdat";

        public uint Version { get; }

        public List<IGlyphBitmap> GlyphBitmaps { get; } = new List<IGlyphBitmap>();

        public BdatTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUInt32();
        }
    }
}