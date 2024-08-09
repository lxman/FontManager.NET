using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameTable1 : IInfoTable, INameTable
    {
        public ushort Format { get; }

        public ushort Count { get; }

        public ushort StringOffset { get; }

        public List<NameRecord> NameRecords { get; } = new List<NameRecord>();

        public ushort LangTagCount { get; }

        public List<LangTagRecord> LangTagRecords { get; } = new List<LangTagRecord>();

        public NameTable1(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Format = reader.ReadUshort();
            Count = reader.ReadUshort();
            StringOffset = reader.ReadUshort();
            LangTagCount = reader.ReadUshort();

            for (var i = 0; i < Count; i++)
            {
                NameRecords.Add(new NameRecord(reader.ReadBytes(NameRecord.RecordSize)));
            }

            for (var i = 0; i < LangTagCount; i++)
            {
                LangTagRecords.Add(new LangTagRecord(reader.ReadBytes(LangTagRecord.RecordSize)));
            }
        }
    }
}
