using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class SequentialMapGroup
    {
        public static long RecordSize => 12;

        public uint StartCharCode { get; }

        public uint EndCharCode { get; }

        public uint StartGlyphId { get; }

        public SequentialMapGroup(byte[] data)
        {
            var reader = new BigEndianReader(data);
            StartCharCode = reader.ReadUint32();
            EndCharCode = reader.ReadUint32();
            StartGlyphId = reader.ReadUint32();
        }

        public override string ToString()
        {
            return $"StartCharCode: {StartCharCode}, EndCharCode: {EndCharCode}, StartGlyphId: {StartGlyphId}";
        }
    }
}