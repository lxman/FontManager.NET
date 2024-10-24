using System;
using System.Collections.Generic;
using System.Linq;
using FontParser.Reader;
using FontParser.Tables.Cff.Type1.Charsets;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace FontParser.Tables.Cff.Type1
{
    public class Type1Table : IFontTable
    {
        public static string Tag => "CFF ";

        public IEncoding Encoding { get; }

        public ICharset CharSet { get; }

        public List<string> Names { get; } = new List<string>();

        public List<string> Strings { get; } = new List<string>();

        public List<List<string>> CharStringList { get; } = new List<List<string>>();

        private readonly Type1TopDictOperatorEntries _type1TopDictOperatorEntries =
            new Type1TopDictOperatorEntries(new Dictionary<ushort, CffDictEntry?>());

        private readonly PrivateDictOperatorEntries _privateDictOperatorEntries =
            new PrivateDictOperatorEntries(new Dictionary<ushort, CffDictEntry?>());

        private readonly List<CffDictEntry> _topDictOperatorEntries = new List<CffDictEntry>();
        private readonly List<CffDictEntry> _type1PrivateDictOperatorEntries = new List<CffDictEntry>();
        private readonly List<List<byte>> _localSubroutines = new List<List<byte>>();

        public Type1Table(byte[] data)
        {
            using var reader = new BigEndianReader(data);

            var header = new Type1Header(reader);

            var nameIndex = new Type1Index(reader);

            foreach (List<byte> bytes in nameIndex.Data)
            {
                Names.Add(System.Text.Encoding.ASCII.GetString(bytes.ToArray()));
            }

            var topDictIndex = new Type1Index(reader);

            ReadTopDictEntries(topDictIndex.Data);

            var stringIndex = new Type1Index(reader);

            foreach (List<byte> bytes in stringIndex.Data)
            {
                Strings.Add(System.Text.Encoding.ASCII.GetString(bytes.ToArray()));
            }

            ResolveDictSids(_topDictOperatorEntries);

            var globalSubrIndex = new Type1Index(reader);
            List<List<byte>> globalSubroutines = globalSubrIndex.Data;

            byte encodingFormat = reader.ReadByte();
            Encoding = encodingFormat switch
            {
                0 => new Encoding0(reader),
                1 => new Encoding1(reader),
                _ => Encoding
            };

            reader.Seek(Convert.ToInt64(_topDictOperatorEntries.First(e => e.Name == "CharStrings").Operand));

            var charStrings = new Type1Index(reader);

            reader.Seek(Convert.ToInt64(_topDictOperatorEntries.First(e => e.Name == "charset").Operand));

            byte charsetFormat = reader.ReadByte();
            CharSet = charsetFormat switch
            {
                0 => new CharsetsFormat0(reader,
                    Convert.ToUInt16(charStrings.Data.Count)),
                1 => new CharsetsFormat1(reader,
                    Convert.ToUInt16(charStrings.Data.Count)),
                2 => new CharsetsFormat2(reader,
                    Convert.ToUInt16(charStrings.Data.Count)),
                _ => CharSet
            };
            var privateDictInfo = (List<double>?)_topDictOperatorEntries.FirstOrDefault(e => e.Name == "Private")?.Operand;
            if (privateDictInfo is null) return;
            reader.Seek(Convert.ToInt64(privateDictInfo[1]));
            double privateDictSize = privateDictInfo[0];
            ReadPrivateDictEntries(reader, privateDictSize);
            CffDictEntry? subrEntry = _type1PrivateDictOperatorEntries.FirstOrDefault(e => e.Name == "Subrs");
            if (subrEntry is null) return;
            reader.Seek(Convert.ToInt64(privateDictInfo[1]) + Convert.ToInt64(subrEntry.Operand));
            ushort localSubrCount = reader.ReadUShort();
            if (localSubrCount == 0) return;
            byte offSize = reader.ReadByte();
            List<uint> localSubrOffsets = reader.ReadOffsets(offSize, localSubrCount + 1u).ToList();
            var subrIndex = 0;
            while (subrIndex < localSubrOffsets.Count - 1)
            {
                _localSubroutines.Add(new List<byte>(reader.ReadBytes(localSubrOffsets[subrIndex + 1] - localSubrOffsets[subrIndex])));
                subrIndex++;
            }

            foreach (
                CharStringParser parser in
                charStrings
                    .Data
                    .Select(bytes =>
                            new CharStringParser(
                                48,
                                bytes,
                                globalSubroutines,
                                _localSubroutines,
                                Convert.ToInt32(_type1PrivateDictOperatorEntries.FirstOrDefault(e => e.Name == "nominalWidthX")?.Operand ?? 0)
                                )
                    )
                )
            {
                CharStringList.Add(parser.Parse());
            }
        }

        private void ReadTopDictEntries(List<List<byte>> data)
        {
            foreach (List<byte> bytes in data)
            {
                DictEntryReader.Read(bytes, _type1TopDictOperatorEntries, _topDictOperatorEntries);
            }
        }

        private void ReadPrivateDictEntries(BigEndianReader reader, double size)
        {
            List<byte> bytes = reader.ReadBytes(Convert.ToInt32(size)).ToList();
            DictEntryReader.Read(bytes, _privateDictOperatorEntries, _type1PrivateDictOperatorEntries);
        }

        private void ResolveDictSids(List<CffDictEntry> entries)
        {
            entries.ForEach(e =>
            {
                if (e.OperandKind != OperandKind.StringId) return;
                var sid = Convert.ToInt32(e.Operand);
                if (sid > StandardStrings.StandardStringsLimit)
                {
                    e.Operand = Strings[sid - StandardStrings.StandardStringsLimit - 1];
                }
                else
                {
                    e.Operand = StandardStrings.GetString(Convert.ToInt32(e.Operand)) ?? sid.ToString();
                }
            });
        }
    }
}