using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos
{
    public class Mark2Record
    {
        public ushort[] Mark2AnchorOffsets { get; }

        public Mark2Record(byte[] data, ushort markClassCount)
        {
            var reader = new BigEndianReader(data);
            Mark2AnchorOffsets = new ushort[markClassCount];
            for (var i = 0; i < markClassCount; i++)
            {
                Mark2AnchorOffsets[i] = reader.ReadUShort();
            }
        }
    }
}
