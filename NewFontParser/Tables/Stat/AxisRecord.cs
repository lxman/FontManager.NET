using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Stat
{
    public class AxisRecord
    {
        public string Tag { get; }

        public ushort AxisNameId { get; }

        public ushort AxisOrdering { get; }

        public AxisRecord(BigEndianReader reader)
        {
            Tag = Encoding.ASCII.GetString(reader.ReadBytes(4));
            AxisNameId = reader.ReadUShort();
            AxisOrdering = reader.ReadUShort();
        }
    }
}
