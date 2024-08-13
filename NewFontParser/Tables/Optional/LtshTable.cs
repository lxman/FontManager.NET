using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional
{
    public class LtshTable : IInfoTable
    {
        public ushort Version { get; }

        public byte[] YPels { get; }

        public LtshTable(byte[] data, ushort numGlyphs)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUShort();
            YPels = reader.ReadBytes(numGlyphs);
        }
    }
}
