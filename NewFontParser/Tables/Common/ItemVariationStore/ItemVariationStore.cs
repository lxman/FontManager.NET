using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ItemVariationStore
{
    public class ItemVariationStore
    {
        public ushort Format { get; }

        public uint VariationRegionListOffset { get; }

        public List<ItemVariationData> ItemVariationData { get; } = new List<ItemVariationData>();

        public ItemVariationStore(BigEndianReader reader)
        {
            long position = reader.Position;

            Format = reader.ReadUShort();
            VariationRegionListOffset = reader.ReadUInt32();
            ushort itemVariationDataCount = reader.ReadUShort();
            var itemVariationDataOffsets = new uint[itemVariationDataCount];
            for (var i = 0; i < itemVariationDataCount; i++)
            {
                itemVariationDataOffsets[i] = reader.ReadUInt32();
            }
            for (var i = 0; i < itemVariationDataCount; i++)
            {
                reader.Seek(position + itemVariationDataOffsets[i]);
                ItemVariationData.Add(new ItemVariationData(reader));
            }
        }
    }
}
