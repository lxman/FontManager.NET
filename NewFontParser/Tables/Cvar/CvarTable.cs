using NewFontParser.Reader;
using NewFontParser.Tables.Common.TupleVariationStore;

namespace NewFontParser.Tables.Cvar
{
    public class CvarTable : IInfoTable
    {
        public static string Tag => "cvar";

        public Header Header { get; }

        public CvarTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Header = new Header(reader, 0, true);
        }
    }
}
