using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class SimpleGlyphHeader
    {
        public long TableSize => 10;

        public short NumberOfContours { get; }

        public short XMin { get; }

        public short YMin { get; }

        public short XMax { get; }

        public short YMax { get; }

        public SimpleGlyphHeader(byte[] data)
        {
            var reader = new BigEndianReader(data);
            NumberOfContours = reader.ReadShort();
            XMin = reader.ReadShort();
            YMin = reader.ReadShort();
            XMax = reader.ReadShort();
            YMax = reader.ReadShort();
        }
    }
}
