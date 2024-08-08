using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameRecord
    {
        public ushort PlatformId { get; }

        public ushort EncodingId { get; }

        public ushort LanguageId { get; }

        public ushort NameId { get; }

        public ushort Length { get; }

        public ushort Offset { get; }

        public string Name { get; }

        public NameRecord(BigEndianReader reader)
        {
            PlatformId = reader.ReadUshort();
            EncodingId = reader.ReadUshort();
            LanguageId = reader.ReadUshort();
            NameId = reader.ReadUshort();
            Length = reader.ReadUshort();
            Offset = reader.ReadUshort();
            //Name = name;
        }
    }
}
