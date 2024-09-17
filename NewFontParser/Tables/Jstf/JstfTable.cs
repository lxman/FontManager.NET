using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfTable : IFontTable
    {
        public static string Tag => "JSTF";

        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public List<JstfScriptRecord> ScriptRecords { get; } = new List<JstfScriptRecord>();

        public JstfTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
            ushort scriptCount = reader.ReadUShort();
            for (var i = 0; i < scriptCount; i++)
            {
                ScriptRecords.Add(new JstfScriptRecord(reader));
            }
        }
    }
}