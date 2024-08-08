using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class UvsMappingRecord
    {
        public static long RecordSize => 5;

        public uint UnicodeValue { get; }

        public ushort GlyphId { get; }

        public UvsMappingRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            UnicodeValue = reader.ReadUint24();
            GlyphId = reader.ReadUshort();
        }

        public override string ToString()
        {
            return $"Unicode Value: {UnicodeValue}, Glyph ID: {GlyphId}";
        }
    }
}