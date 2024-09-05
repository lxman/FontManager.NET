using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.Common
{
    public class MarkRecord
    {
        public ushort MarkClass { get; }

        public short MarkAnchorOffset { get; }

        public MarkRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);

            MarkClass = reader.ReadUShort();
            MarkAnchorOffset = reader.ReadShort();
        }
    }
}