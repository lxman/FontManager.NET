using NewFontParser.Reader;

namespace NewFontParser.Tables.Fftm
{
    public class Table : IInfoTable
    {
        public uint Version { get; }

        public long FFTimestamp { get; }

        public long CreatedFFTimestamp { get; }

        public long ModifiedFFTimestamp { get; }

        public Table(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUInt32();
            FFTimestamp = reader.ReadLong();
            CreatedFFTimestamp = reader.ReadLong();
            ModifiedFFTimestamp = reader.ReadLong();
        }
    }
}
