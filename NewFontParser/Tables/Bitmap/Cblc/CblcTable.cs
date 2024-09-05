using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Cblc
{
    public class CblcTable : IInfoTable
    {
        public static string Tag => "CBLC";

        public CblcTable(byte[] data)
        {
            // TODO: Implement
            var reader = new BigEndianReader(data);
        }
    }
}