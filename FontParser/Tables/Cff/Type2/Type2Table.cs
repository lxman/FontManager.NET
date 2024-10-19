using System;
using System.Collections.Generic;
using System.Linq;
using FontParser.Reader;
using FontParser.Tables.Cff.Type1;
using FontParser.Tables.Cff.Type2.FontDictSelect;
using FontParser.Tables.Common.ItemVariationStore;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace FontParser.Tables.Cff.Type2
{
    public class Type2Table : IFontTable
    {
        public static string Tag => "CFF2";

        public Type2Header Header { get; private set; }

        public List<List<byte>> GlobalSubroutines { get; private set; }

        public List<CffDictEntry> TopDictOperatorEntries { get; } = new List<CffDictEntry>();

        public List<CffDictEntry> PrivateDictOperatorEntries { get; } = new List<CffDictEntry>();

        private readonly Type2TopDictOperatorEntries _type2TopDictOperatorEntries =
            new Type2TopDictOperatorEntries(new Dictionary<ushort, CffDictEntry?>());
        private readonly Type2FontDictEntries _type2FontDictEntries =
            new Type2FontDictEntries(new Dictionary<ushort, CffDictEntry?>());
        private readonly BigEndianReader _reader;
        private readonly PrivateDictOperatorEntries _privateDictOperatorEntries =
            new PrivateDictOperatorEntries(new Dictionary<ushort, CffDictEntry?>());

        public Type2Table(byte[] data)
        {
            _reader = new BigEndianReader(data);
        }

        public void Process(ushort numGlyphs)
        {
            Header = new Type2Header(_reader);

            _reader.Seek(Header.HeaderSize);

            ReadTopDictEntries(_reader, Header.TopDictSize);

            _reader.Seek(Header.HeaderSize + Header.TopDictSize);

            var gsIndex = new Type2Index(_reader);
            GlobalSubroutines = gsIndex.Data;

            _reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "CharStringIndexOffset").Operand));
            var csIndex = new Type2Index(_reader);

            CffDictEntry? entry = TopDictOperatorEntries.FirstOrDefault(e => e.Name == "FontDictSelectOffset");
            IFdSelect? fdSelect = null;
            if (!(entry is null))
            {
                _reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "FontDictSelectOffset").Operand));
                fdSelect = _reader.PeekBytes(1)[0] switch
                {
                    0 => new FdsFormat0(_reader, numGlyphs),
                    3 => new FdsFormat3(_reader),
                    4 => new FdsFormat4(_reader),
                    _ => fdSelect
                };
            }

            ItemVariationStore? itemVariationStore = null;
            long? vsOffset =
                Convert.ToInt64(TopDictOperatorEntries.FirstOrDefault(e => e.Name == "VariationStoreOffset")?.Operand ?? -1);
            if (vsOffset > 0)
            {
                // Why do I have to add 2 to make it work?
                _reader.Seek(vsOffset.Value + 2);
                itemVariationStore = new ItemVariationStore(_reader);
            }

            _reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "FontDictIndexOffset").Operand));
            var fdIndex = new Type2Index(_reader);
            var fontDicts = new List<CffDictEntry>();
            fdIndex.Data.ForEach(bytes =>
            {
                var dict = new List<CffDictEntry>();
                DictEntryReader.Read(bytes,
                    _type2FontDictEntries,
                    dict);
                fontDicts.AddRange(dict);
            });
            var privateDicts = new List<List<CffDictEntry>>();
            fontDicts.ForEach(cffDictEntry =>
            {
                if (!(cffDictEntry.Operand is List<double> data)) return;
                double size = data[0];
                double offset = data[1];
                _reader.Seek(Convert.ToInt64(offset));
                var dict = new List<CffDictEntry>();
                ReadPrivateDictEntries(_reader, size, dict, itemVariationStore?.ItemVariationData[0].RegionIndexes ?? new List<ushort>());
                privateDicts.Add(dict);
            });
        }

        private void ReadTopDictEntries(BigEndianReader reader, ushort size)
        {
            List<byte> bytes = reader.ReadBytes(Convert.ToInt32(size)).ToList();
            DictEntryReader.Read(bytes, _type2TopDictOperatorEntries, TopDictOperatorEntries);
        }

        private void ReadPrivateDictEntries(
            BigEndianReader reader,
            double size,
            List<CffDictEntry> entries,
            List<ushort> activeVariationRegionData)
        {
            List<byte> bytes = reader.ReadBytes(Convert.ToInt32(size)).ToList();
            DictEntryReader.Read(bytes, _privateDictOperatorEntries, entries, activeVariationRegionData);
        }
    }
}
