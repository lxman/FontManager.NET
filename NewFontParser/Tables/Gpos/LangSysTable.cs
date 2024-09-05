using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos
{
    public class LangSysTable
    {
        public ushort LookupOrder { get; }

        public ushort RequiredFeatureIndex { get; }

        public ushort FeatureIndexCount { get; }

        public ushort[] FeatureIndices { get; }

        public LangSysTable(BigEndianReader reader)
        {
            LookupOrder = reader.ReadUShort();
            RequiredFeatureIndex = reader.ReadUShort();
            FeatureIndexCount = reader.ReadUShort();

            FeatureIndices = new ushort[FeatureIndexCount];
            for (var i = 0; i < FeatureIndexCount; i++)
            {
                FeatureIndices[i] = reader.ReadUShort();
            }
        }
    }
}