using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.ItemVariationStore;

namespace NewFontParser.Tables.Colr
{
    public class ColrTable : IInfoTable
    {
        public static string Tag => "COLR";

        public ushort Version { get; }

        public List<BaseGlyphRecord> BaseGlyphRecords { get; } = new List<BaseGlyphRecord>();

        public List<LayerRecord> LayerRecords { get; } = new List<LayerRecord>();

        public BaseGlyphList? BaseGlyphList { get; }

        public LayerList? LayerList { get; }

        public ClipList? ClipList { get; }

        public DeltaSetIndexMap? DeltaSetIndexMap { get; }

        public ItemVariationStore? ItemVariationStore { get; }

        public ColrTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUShort();
            ushort baseGlyphRecordCount = reader.ReadUShort();
            uint baseGlyphRecordOffset = reader.ReadUInt32();
            uint layerRecordOffset = reader.ReadUInt32();
            ushort layerRecordCount = reader.ReadUShort();
            reader.Seek(baseGlyphRecordOffset);
            for (var i = 0; i < baseGlyphRecordCount; i++)
            {
                BaseGlyphRecords.Add(new BaseGlyphRecord(reader));
            }
            reader.Seek(layerRecordOffset);
            for (var i = 0; i < layerRecordCount; i++)
            {
                LayerRecords.Add(new LayerRecord(reader));
            }
            if (Version == 0) return;
            uint baseGlyphListOffset = reader.ReadUInt32();
            reader.Seek(baseGlyphListOffset);
            BaseGlyphList = new BaseGlyphList(reader);
            uint layerListOffset = reader.ReadUInt32();
            reader.Seek(layerListOffset);
            LayerList = new LayerList(reader);
            uint clipListOffset = reader.ReadUInt32();
            reader.Seek(clipListOffset);
            ClipList = new ClipList(reader);
            uint deltaSetIndexMapOffset = reader.ReadUInt32();
            reader.Seek(deltaSetIndexMapOffset);
            DeltaSetIndexMap = new DeltaSetIndexMap(reader);
            uint itemVariationStoreOffset = reader.ReadUInt32();
            reader.Seek(itemVariationStoreOffset);
            ItemVariationStore = new ItemVariationStore(reader);
        }
    }
}