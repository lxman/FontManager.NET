using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameRecord
    {
        public static long RecordSize => 12;

        public ushort PlatformId { get; }

        public ushort EncodingId { get; }

        public ushort LanguageId { get; }

        public ushort NameId { get; }

        public ushort Length { get; }

        public ushort Offset { get; }

        //public string Name { get; }

        public NameRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            PlatformId = reader.ReadUShort();
            EncodingId = reader.ReadUShort();
            LanguageId = reader.ReadUShort();
            NameId = reader.ReadUShort();
            Length = reader.ReadUShort();
            Offset = reader.ReadUShort();
            //Name = name;
        }
    }
}
