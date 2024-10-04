using FontParser.Reader;

namespace FontParser.Tables.Hmtx
{
    public class LongHMetricRecord
    {
        public static long RecordSize => 4;

        public ushort AdvanceWidth { get; }

        public short LeftSideBearing { get; }

        public LongHMetricRecord(BigEndianReader reader)
        {
            AdvanceWidth = reader.ReadUShort();
            LeftSideBearing = reader.ReadShort();
        }
    }
}