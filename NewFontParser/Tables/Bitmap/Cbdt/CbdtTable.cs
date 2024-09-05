using System.IO;

namespace NewFontParser.Tables.Bitmap.Cbdt
{
    public class CbdtTable : IInfoTable
    {
        public static string Tag => "CBDT";

        public CbdtTable(byte[] data)
        {
            // TODO: Implement
            var reader = new BinaryReader(new MemoryStream(data));
        }
    }
}