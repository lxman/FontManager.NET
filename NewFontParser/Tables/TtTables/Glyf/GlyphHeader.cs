using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class GlyphHeader
    {
        public static long RecordSize => 10;

        public short NumberOfContours { get; }

        public short XMin { get; }

        public short YMin { get; }

        public short XMax { get; }

        public short YMax { get; }

        public GlyphHeader(byte[] data)
        {
            using var reader = new BigEndianReader(data);
            NumberOfContours = reader.ReadShort();
            XMin = reader.ReadShort();
            YMin = reader.ReadShort();
            XMax = reader.ReadShort();
            YMax = reader.ReadShort();
        }

        public override string ToString()
        {
            return $"Number of Contours: {NumberOfContours}, XMin: {XMin}, YMin: {YMin}, XMax: {XMax}, YMax: {YMax}";
        }
    }
}