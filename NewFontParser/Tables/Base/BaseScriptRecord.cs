using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Base
{
    public class BaseScriptRecord
    {
        public string BaseScriptTag { get; }

        public BaseScriptTable BaseScriptTable { get; }

        public BaseScriptRecord(BigEndianReader reader, long origin, byte[] tag, ushort offset)
        {
            BaseScriptTag = Encoding.UTF8.GetString(tag);
            reader.Seek(offset + origin);
            BaseScriptTable = new BaseScriptTable(reader);
        }
    }
}
