using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class ScriptList
    {
        public ushort ScriptCount { get; }

        public ScriptRecord[] ScriptRecords { get; }

        private readonly ushort _scriptListOffset;

        public ScriptList(BigEndianReader reader, ushort offset)
        {
            reader.Seek(offset);
            _scriptListOffset = offset;

            ScriptCount = reader.ReadUShort();

            ScriptRecords = new ScriptRecord[ScriptCount];
            for (var i = 0; i < ScriptCount; i++)
            {
                ScriptRecords[i] = new ScriptRecord(reader, _scriptListOffset);
            }
        }
    }
}
