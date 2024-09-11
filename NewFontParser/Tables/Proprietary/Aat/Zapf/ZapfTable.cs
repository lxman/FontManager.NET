using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Zapf
{
    public class ZapfTable : IInfoTable
    {
        public static string Tag => "Zapf";

        public ZapfTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
        }
    }
}
