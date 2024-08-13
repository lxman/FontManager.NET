using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional.Dsig
{
    public class SignatureBlock1
    {
        public ushort Reserved1 { get; }

        public ushort Reserved2 { get; }

        public ushort SignatureLength { get; }

        public byte[] Signature { get; set; }

        public SignatureBlock1(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Reserved1 = reader.ReadUShort();
            Reserved2 = reader.ReadUShort();
            SignatureLength = reader.ReadUShort();
            Signature = reader.ReadBytes(SignatureLength);
        }
    }
}
