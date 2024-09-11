using NewFontParser.Reader;

namespace NewFontParser.Tables.Todo.Arabic.Tsis
{
    public class TsisTable : IInfoTable
    {
        public static string Tag => "TSIS";

        public TsisTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            // TODO: Implement TsisTable
            // This is a proprietary table that seems to be for Arabic fonts
        }
    }
}
