using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameTable : IInfoTable
    {
        public static string Tag => "name";

        public ushort Format { get; }

        public ushort Count { get; }

        public ushort StringStorageOffset { get; }

        public List<NameRecord> NameRecords { get; } = new List<NameRecord>();

        public ushort? LangTagCount { get; }

        public List<LangTagRecord>? LangTagRecords { get; }

        public NameTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Format = reader.ReadUShort();
            Count = reader.ReadUShort();
            StringStorageOffset = reader.ReadUShort();
            for (var i = 0; i < Count; i++)
            {
                NameRecords.Add(new NameRecord(reader.ReadBytes(NameRecord.RecordSize)));
            }
            if (Format == 0) return;
            LangTagCount = reader.ReadUShort();
            if (LangTagCount == null) return;
            LangTagRecords = new List<LangTagRecord>();
            for (var i = 0; i < LangTagCount; i++)
            {
                LangTagRecords.Add(new LangTagRecord(reader.ReadBytes(LangTagRecord.RecordSize)));
            }
        }
    }
}
