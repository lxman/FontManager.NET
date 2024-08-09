using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class GlyphHeader
    {
        public short NumberOfContours { get; }

        public short XMin { get; }

        public short YMin { get; }

        public short XMax { get; }

        public short YMax { get; }

        public GlyphHeader(byte[] data)
        {
            var reader = new BigEndianReader(data);
            NumberOfContours = reader.ReadShort();
            XMin = reader.ReadShort();
            YMin = reader.ReadShort();
            XMax = reader.ReadShort();
            YMax = reader.ReadShort();
            if (NumberOfContours < 0)
            {
                var compositeGlyphHeader = new CompositeGlyphHeader(data);
            }
            else
            if (NumberOfContours == 0)
            {
                var compositeGlyphHeader = new CompositeGlyphHeader(data);
            }
            else
            {
                var simpleGlyphHeader = new SimpleGlyphHeader(data);
            }
        }
    }
}
