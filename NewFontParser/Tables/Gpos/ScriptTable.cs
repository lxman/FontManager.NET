using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos
{
    public class ScriptTable
    {
        public ushort DefaultLangSysOffset { get; }

        public ushort ScriptCount { get; }

        public LangSysRecord[] ScriptRecords { get; }

        public ScriptTable(BigEndianReader reader, long offset)
        {
            reader.Seek(offset);

            DefaultLangSysOffset = reader.ReadUShort();
            ScriptCount = reader.ReadUShort();

            ScriptRecords = new LangSysRecord[ScriptCount];
            for (var i = 0; i < ScriptCount; i++)
            {
                ScriptRecords[i] = new LangSysRecord(reader);
            }
        }
    }
}