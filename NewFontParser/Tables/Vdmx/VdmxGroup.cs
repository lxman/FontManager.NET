using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Vdmx
{
    public class VdmxGroup
    {
        public ushort RecordCount { get; }

        public byte StartSize { get; }

        public byte EndSize { get; }

        public List<VTable> Records { get; } = new List<VTable>();

        public VdmxGroup(byte[] data)
        {
            var reader = new BigEndianReader(data);

            RecordCount = reader.ReadUShort();
            StartSize = reader.ReadByte();
            EndSize = reader.ReadByte();

            for (var i = 0; i < RecordCount; i++)
            {
                Records.Add(new VTable(reader));
            }
        }
    }
}