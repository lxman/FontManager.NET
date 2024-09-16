using NewFontParser.Reader;

namespace NewFontParser.Tables.Todo.Arabic.Tsif
{
    public class TsifTable : IInfoTable
    {
        public static string Tag => "TSIF";

        public TsifTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            // TODO: Implement TSIF table parsing
            // This is a proprietary table for Arabic fonts
        }
    }
}