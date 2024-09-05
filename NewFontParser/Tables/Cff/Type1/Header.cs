using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1
{
    public class Header
    {
        public byte MajorVersion { get; }

        public byte MinorVersion { get; }

        public byte HeaderSize { get; }

        public byte OffSize { get; }

        public Header(BigEndianReader reader)
        {
            MajorVersion = reader.ReadByte();
            MinorVersion = reader.ReadByte();
            HeaderSize = reader.ReadByte();
            OffSize = reader.ReadByte();
        }
    }
}