using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr
{
    public class ItemVariationData
    {
        public ushort WordDeltaCount { get; }

        public ushort RegionIndexCount { get; }

        public ushort[] RegionIndexes { get; }

        public ItemVariationData(BigEndianReader reader)
        {
            WordDeltaCount = reader.ReadUShort();
            RegionIndexCount = reader.ReadUShort();
            RegionIndexes = new ushort[RegionIndexCount];
            for (var i = 0; i < RegionIndexCount; i++)
            {
                RegionIndexes[i] = reader.ReadUShort();
            }
        }
    }
}
