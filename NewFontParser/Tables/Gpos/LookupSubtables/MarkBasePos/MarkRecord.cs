using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkBasePos
{
    public class MarkRecord
    {
        public ushort MarkClass { get; }

        public ushort MarkAnchorOffset { get; }

        public MarkRecord(byte[] data)
        {
            using var reader = new BigEndianReader(data);

            MarkClass = reader.ReadUShort();
            MarkAnchorOffset = reader.ReadUShort();
        }
    }
}