using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Cbdt
{
    public class CbdtTable : IFontTable
    {
        public static string Tag => "CBDT";

        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public CbdtTable(byte[] data)
        {
            // TODO: Implement
            var reader = new BigEndianReader(data);
            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
        }
    }
}