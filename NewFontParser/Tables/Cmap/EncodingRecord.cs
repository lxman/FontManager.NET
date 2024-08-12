using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap
{
    internal class EncodingRecord
    {
        public static long RecordSize => 8;

        public ushort PlatformId { get; private set; }

        public ushort EncodingId { get; private set; }

        public uint Offset { get; private set; }

        public EncodingRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            PlatformId = reader.ReadUShort();
            EncodingId = reader.ReadUShort();
            Offset = reader.ReadUint32();
        }
    }
}