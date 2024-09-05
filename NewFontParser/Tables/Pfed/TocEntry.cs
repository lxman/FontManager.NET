using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Pfed
{
    public class TocEntry
    {
        public string Tag { get; }

        public uint Offset { get; }

        public TocEntry(BigEndianReader reader)
        {
            Tag = Encoding.ASCII.GetString(reader.ReadBytes(4));
            Offset = reader.ReadUInt32();
        }
    }
}