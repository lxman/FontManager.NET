using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos
{
    public class ScriptTable
    {
        public ushort DefaultLangSysOffset { get; }

        public ushort ScriptCount { get; }

        public ScriptRecord[] ScriptRecords { get; }

        public ScriptTable(BigEndianReader reader, ushort offset)
        {
            reader.Seek(offset);

            DefaultLangSysOffset = reader.ReadUShort();
            ScriptCount = reader.ReadUShort();

            ScriptRecords = new ScriptRecord[ScriptCount];
            for (var i = 0; i < ScriptCount; i++)
            {
                ScriptRecords[i] = new ScriptRecord(reader);
            }
        }
    }
}
