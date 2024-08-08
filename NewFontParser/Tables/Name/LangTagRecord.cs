using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class LangTagRecord
    {
        public static long RecordSize => 4;

        public ushort Length { get; }

        public ushort Offset { get; }

        public string Tag { get; }

        public LangTagRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Length = reader.ReadUshort();
            Offset = reader.ReadUshort();
            //Tag = tag;
        }
    }
}
