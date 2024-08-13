using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameRecord
    {
        public static long RecordSize => 12;

        public PlatformId PlatformId { get; }

        public Enum EncodingId { get; }

        public ushort LanguageId { get; }

        public string NameId { get; }

        public ushort Length { get; }

        public ushort Offset { get; }

        public string? Name { get; set; }

        public NameRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            PlatformId = (PlatformId)reader.ReadUShort();
            switch (PlatformId)
            {
                case PlatformId.Unicode:
                    EncodingId = (Platform0EncodingId)reader.ReadUShort();
                    break;
                case PlatformId.Macintosh:
                    EncodingId = (MacintoshEncodingId)reader.ReadUShort();
                    break;
                case PlatformId.Iso:
                    EncodingId = (Platform2EncodingId)reader.ReadUShort();
                    break;
                case PlatformId.Windows:
                    EncodingId = (Platform3EncodingId)reader.ReadUShort();
                    break;
                case PlatformId.Custom:
                    _ = reader.ReadUShort();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            LanguageId = reader.ReadUShort();
            NameId = NameIdTranslator.Translate(reader.ReadUShort());
            Length = reader.ReadUShort();
            Offset = reader.ReadUShort();
        }
    }
}
