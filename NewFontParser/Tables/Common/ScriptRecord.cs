using System.Text;
using NewFontParser.Reader;
using NewFontParser.Tables.Gpos;

namespace NewFontParser.Tables.Common
{
    public class ScriptRecord
    {
        public string ScriptTag { get; }

        public ScriptTable ScriptTable { get; }

        public ScriptRecord(BigEndianReader reader, long offset)
        {
            ScriptTag = Encoding.UTF8.GetString(reader.ReadBytes(4));
            ushort scriptOffset = reader.ReadUShort();
            long position = reader.Position;
            ScriptTable = new ScriptTable(reader, scriptOffset + offset);
            reader.Seek(position);
        }
    }
}