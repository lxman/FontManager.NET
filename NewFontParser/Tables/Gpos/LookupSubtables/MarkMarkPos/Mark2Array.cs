using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos
{
    public class Mark2Array
    {
        public Mark2Record[] Mark2Records { get; }

        public Mark2Array(BigEndianReader reader, ushort markClassCount)
        {
            ushort mark2Count = reader.ReadUShort();
            Mark2Records = new Mark2Record[mark2Count];
            for (var i = 0; i < mark2Count; i++)
            {
                Mark2Records[i] = new Mark2Record(reader.ReadBytes(2 * markClassCount), markClassCount);
            }
        }
    }
}