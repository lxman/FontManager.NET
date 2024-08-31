using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional.Dsig
{
    public class DsigTable : IInfoTable
    {
        public static string Tag => "DSIG";

        public uint Version { get; }

        public ushort NumSigs { get; }

        public PermissionFlags PermissionFlags { get; }

        public List<SigRecord> SigRecords { get; } = new List<SigRecord>();

        public DsigTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUInt32();
            NumSigs = reader.ReadUShort();
            if (NumSigs == 0) return;
            PermissionFlags = (PermissionFlags)reader.ReadUShort();

            for (var i = 0; i < NumSigs; i++)
            {
                SigRecords.Add(new SigRecord(reader));
            }

            SigRecords.ForEach(r => r.ReadSignature(reader));
        }
    }
}
