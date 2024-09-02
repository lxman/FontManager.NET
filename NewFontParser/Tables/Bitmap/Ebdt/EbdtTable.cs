using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Ebdt
{
    public class EbdtTable : IInfoTable
    {
        public static string Tag => "EBDT";

        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public EbdtTable(byte[] bytes)
        {
            var reader = new BigEndianReader(bytes);
            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
        }
    }
}
