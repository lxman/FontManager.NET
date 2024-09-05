using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class FeatureRecord
    {
        public ushort FeatureIndex { get; }

        public ushort FeatureTag { get; }

        public ushort Offset { get; }

        public FeatureRecord(BigEndianReader reader)
        {
            FeatureIndex = reader.ReadUShort();
            FeatureTag = reader.ReadUShort();
            Offset = reader.ReadUShort();
        }
    }
}