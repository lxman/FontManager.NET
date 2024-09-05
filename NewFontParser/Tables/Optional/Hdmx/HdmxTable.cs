using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional.Hdmx
{
    public class HdmxTable : IInfoTable
    {
        public static string Tag => "hdmx";

        public ushort Version { get; private set; }

        public short NumRecords { get; private set; }

        public int RecordSize { get; private set; }

        public List<HdmxRecord> Records { get; } = new List<HdmxRecord>();

        private readonly BigEndianReader _reader;

        public HdmxTable(byte[] data)
        {
            _reader = new BigEndianReader(data);
        }

        // numGlyphs is from the maxp table
        public void Process(ushort numGlyphs)
        {
            Version = _reader.ReadUShort();
            NumRecords = _reader.ReadShort();
            RecordSize = _reader.ReadInt32();

            for (var i = 0; i < NumRecords; i++)
            {
                Records.Add(new HdmxRecord(_reader, numGlyphs));
            }
        }
    }
}