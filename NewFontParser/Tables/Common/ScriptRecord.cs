using System;
using System.Text;
using NewFontParser.Reader;
using NewFontParser.Tables.Gpos;

namespace NewFontParser.Tables.Common
{
    public class ScriptRecord
    {
        public string ScriptTag { get; }

        public ushort ScriptOffset { get; }

        public ScriptTable ScriptTable { get; }

        private readonly ushort _scriptListOffset;

        public ScriptRecord(BigEndianReader reader, ushort offset)
        {
            _scriptListOffset = offset;
            ScriptTag = Encoding.UTF8.GetString(reader.ReadBytes(4));
            ScriptOffset = reader.ReadUShort();
            long position = reader.Position;
            ScriptTable = new ScriptTable(reader, Convert.ToUInt16(ScriptOffset + _scriptListOffset));
            reader.Seek(position);
        }
    }
}
