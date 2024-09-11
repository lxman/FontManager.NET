using NewFontParser.Reader;

namespace NewFontParser.Tables.Todo.Arabic.Tsiv
{
    public class TsivTable : IInfoTable
    {
        public static string Tag => "TSIV";

        public TsivTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            // TODO: Implement
            // TsivTable is a proprietary table in the Arabic script.
        }
    }
}
