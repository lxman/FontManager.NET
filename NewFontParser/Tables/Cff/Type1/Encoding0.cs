using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1
{
    public class Encoding0 : IEncoding
    {
        public byte Format => 0;

        public byte[] CodeArray { get; }

        public Encoding0(BigEndianReader reader)
        {
            byte nCodes = reader.ReadByte();
            CodeArray = reader.ReadBytes(nCodes);
        }
    }
}