using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos
{
    public class GposTable : IInfoTable
    {
        public GposHeader Header { get; }

        public GposTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Header = new GposHeader(reader);
        }
    }
}
