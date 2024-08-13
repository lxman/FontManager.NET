using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional.Dsig
{
    public class DsigTable : IInfoTable
    {
        public ushort Version { get; set; }

        public ushort NumSigs { get; set; }

        public List<SigRecord> SigRecords { get; set; } = new List<SigRecord>();

        public DsigTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUShort();
            NumSigs = reader.ReadUShort();

            for (var i = 0; i < NumSigs; i++)
            {
                var record = new SigRecord
                {
                    Format = reader.ReadUShort(),
                    Length = reader.ReadUShort(),
                    Offset = reader.ReadUShort()
                };

                SigRecords.Add(record);
            }
        }
    }
}
