using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ItemVariationStore
{
    public class ItemVariationData
    {
        public ushort WordDeltaCount { get; }

        public ushort[] RegionIndexes { get; }
        
        public DeltaSetRecord[] DeltaSets { get; }

        public ItemVariationData(BigEndianReader reader, bool useLongWords)
        {
            ushort itemCount = reader.ReadUShort();
            WordDeltaCount = reader.ReadUShort();
            bool longWords = (WordDeltaCount & 0x8000) == 1;
            int deltaCount = WordDeltaCount & 0x7FFF;
            ushort regionIndexCount = reader.ReadUShort();
            RegionIndexes = reader.ReadUShortArray(regionIndexCount);
            DeltaSets = new DeltaSetRecord[itemCount];
            for (var i = 0; i < itemCount; i++)
            {
                DeltaSets[i] = new DeltaSetRecord(
                    reader,
                    regionIndexCount,
                    useLongWords && longWords,
                    deltaCount);
            }
        }
    }
}