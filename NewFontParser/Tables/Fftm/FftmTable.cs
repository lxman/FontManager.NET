using NewFontParser.Reader;

namespace NewFontParser.Tables.Fftm
{
    public class FftmTable : IInfoTable
    {
        public static string Tag => "FFTM";

        public uint Version { get; }

        public long FFTimestamp { get; }

        public long CreatedFFTimestamp { get; }

        public long ModifiedFFTimestamp { get; }

        public FftmTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUInt32();
            FFTimestamp = reader.ReadLong();
            CreatedFFTimestamp = reader.ReadLong();
            ModifiedFFTimestamp = reader.ReadLong();
        }
    }
}
