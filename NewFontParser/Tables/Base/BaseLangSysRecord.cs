using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Base
{
    public class BaseLangSysRecord
    {
        public string LangSysTag { get; }

        public MinMaxTable MinMaxTable { get; }

        public BaseLangSysRecord(BigEndianReader reader, long origin)
        {
            LangSysTag = Encoding.UTF8.GetString(reader.ReadBytes(4));
            ushort minMaxOffset = reader.ReadUShort();
            reader.Seek(minMaxOffset + origin);
            MinMaxTable = new MinMaxTable(reader);
        }
    }
}