using NewFontParser.Reader;

namespace NewFontParser.Tables.Hmtx
{
    public class LongHMetricRecord
    {
        public static long RecordSize => 4;

        public ushort AdvanceWidth { get; }

        public short LeftSideBearing { get; }

        public LongHMetricRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            AdvanceWidth = reader.ReadUshort();
            LeftSideBearing = reader.ReadShort();
        }
    }
}
