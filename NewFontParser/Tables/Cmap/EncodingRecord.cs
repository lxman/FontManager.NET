using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap
{
    internal class EncodingRecord
    {
        public static long RecordSize => 8;

        public PlatformId PlatformId { get; }

        public Platform0EncodingId? EncodingId0 { get; }

        public Platform1EncodingId? EncodingId1 { get; }

        public Platform2EncodingId? EncodingId2 { get; }

        public Platform3EncodingId? EncodingId3 { get; }

        public uint Offset { get; }

        public EncodingRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            PlatformId = (PlatformId)reader.ReadUShort();
            ushort platformEncodingId = reader.ReadUShort();
            switch (PlatformId)
            {
                case PlatformId.Unicode:
                    EncodingId0 = (Platform0EncodingId)platformEncodingId;
                    break;

                case PlatformId.Macintosh:
                    EncodingId1 = (Platform1EncodingId)platformEncodingId;
                    break;

                case PlatformId.Iso:
                    EncodingId2 = (Platform2EncodingId)platformEncodingId;
                    break;

                case PlatformId.Windows:
                    EncodingId3 = (Platform3EncodingId)platformEncodingId;
                    break;
            }
            Offset = reader.ReadUInt32();
        }
    }
}