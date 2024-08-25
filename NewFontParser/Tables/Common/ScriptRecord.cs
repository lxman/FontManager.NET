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

        public ScriptRecord(BigEndianReader reader)
        {
            long position = reader.Position;

            ScriptTag = Encoding.UTF8.GetString(reader.ReadBytes(4));
            ScriptOffset = reader.ReadUShort();
            reader.Seek(position);
            ScriptTable = new ScriptTable(reader, Convert.ToUInt16(ScriptOffset + position));
        }
    }
}
