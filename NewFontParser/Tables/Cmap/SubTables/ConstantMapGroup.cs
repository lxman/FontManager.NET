using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class ConstantMapGroup
    {
        public static long RecordSize => 12;

        public uint StartCharCode { get; }

        public uint EndCharCode { get; }

        public uint GlyphId { get; }

        public ConstantMapGroup(byte[] data)
        {
            var reader = new BigEndianReader(data);
            StartCharCode = reader.ReadUint32();
            EndCharCode = reader.ReadUint32();
            GlyphId = reader.ReadUint32();
        }
    }
}