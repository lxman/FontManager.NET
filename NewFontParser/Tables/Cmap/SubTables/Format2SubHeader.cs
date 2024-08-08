using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format2SubHeader
    {
        public static long RecordSize => 8;

        public ushort FirstCode { get; }

        public ushort EntryCount { get; }

        public short IdDelta { get; }

        public ushort IdRangeOffset { get; }

        public Format2SubHeader(byte[] data)
        {
            var reader = new BigEndianReader(data);
            FirstCode = reader.ReadUshort();
            EntryCount = reader.ReadUshort();
            IdDelta = reader.ReadShort();
            IdRangeOffset = reader.ReadUshort();
        }
    }
}