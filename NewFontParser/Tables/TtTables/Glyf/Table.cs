using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class Table : IInfoTable
    {
        public Table(byte[] data)
        {
            var reader = new BigEndianReader(data);
            var glyphHeader = new GlyphHeader(data);
        }
    }
}
