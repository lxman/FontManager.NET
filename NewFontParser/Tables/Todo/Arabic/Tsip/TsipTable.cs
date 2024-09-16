using NewFontParser.Reader;

namespace NewFontParser.Tables.Todo.Arabic.Tsip
{
    public class TsipTable : IInfoTable
    {
        public static string Tag => "TSIP";

        public TsipTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            // TODO: Implement TSIP table parsing
            // TSIP table is a proprietary table used by Arabic fonts.
        }
    }
}