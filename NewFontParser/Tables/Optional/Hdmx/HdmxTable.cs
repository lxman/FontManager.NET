using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional.Hdmx
{
    public class HdmxTable : IInfoTable
    {
        public ushort Version { get; }

        public short NumRecords { get; }

        public int RecordSize { get; }

        public List<HdmxRecord> Records { get; } = new List<HdmxRecord>();


        // numGlyphs is from the maxp table
        public HdmxTable(byte[] data, ushort numGlyphs)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUShort();
            NumRecords = reader.ReadShort();
            RecordSize = reader.ReadInt32();

            for (var i = 0; i < NumRecords; i++)
            {
                Records.Add(new HdmxRecord(reader, numGlyphs));
            }
        }
    }
}
