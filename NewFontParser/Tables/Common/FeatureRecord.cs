using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class FeatureRecord
    {
        public string FeatureTag { get; }

        public FeatureTable FeatureTable { get; }

        public FeatureRecord(BigEndianReader reader, long startOfTable)
        {
            FeatureTag = Encoding.ASCII.GetString(reader.ReadBytes(4));
            ushort offset = reader.ReadUShort();
            long before = reader.Position;
            reader.Seek(startOfTable + offset);
            FeatureTable = new FeatureTable(reader);
            reader.Seek(before);
        }
    }
}