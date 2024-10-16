using System;
using System.Collections.Generic;
using System.Linq;
using FontParser.Reader;
using FontParser.Tables.Cff.Type1;

namespace FontParser.Tables.Cff.Type2
{
    public class Type2Table : IFontTable
    {
        public static string Tag => "CFF2";

        public Type2Header Header { get; }

        public List<CffDictEntry> TopDictOperatorEntries { get; } = new List<CffDictEntry>();

        private readonly Type2TopDictOperatorEntries _type2TopDictOperatorEntries =
            new Type2TopDictOperatorEntries(new Dictionary<ushort, CffDictEntry?>());

        public Type2Table(byte[] data)
        {
            using var reader = new BigEndianReader(data);

            Header = new Type2Header(reader);

            reader.Seek(Header.HeaderSize);

            ReadTopDictEntries(reader, Header.TopDictSize);
        }

        private void ReadTopDictEntries(BigEndianReader reader, ushort size)
        {
            List<byte> bytes = reader.ReadBytes(Convert.ToInt32(size)).ToList();
            DictEntryReader.Read(bytes, _type2TopDictOperatorEntries, TopDictOperatorEntries);
        }
    }
}
