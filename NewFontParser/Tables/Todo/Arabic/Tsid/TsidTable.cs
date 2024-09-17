using NewFontParser.Reader;

namespace NewFontParser.Tables.Todo.Arabic.Tsid
{
    public class TsidTable : IFontTable
    {
        public static string Tag => "TSID";

        public TsidTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            // TODO: Implement
            // This is a proprietary table for Arabic fonts
        }
    }
}