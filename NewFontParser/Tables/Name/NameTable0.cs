using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameTable0 : IInfoTable, INameTable
    {
        public ushort Format { get; }

        public ushort Count { get; }

        public ushort StringOffset { get; }

        public List<NameRecord> NameRecords { get; } = new List<NameRecord>();

        public NameTable0(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Format = reader.ReadUShort();
            Count = reader.ReadUShort();
            StringOffset = reader.ReadUShort();

            for (var i = 0; i < Count; i++)
            {
                NameRecords.Add(new NameRecord(reader.ReadBytes(NameRecord.RecordSize)));
            }
        }
    }
}
