using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfScriptRecord
    {
        public string Tag { get; }

        public JstfScript JstfScript { get; }

        public JstfScriptRecord(BigEndianReader reader)
        {
            Tag = Encoding.ASCII.GetString(reader.ReadBytes(4));
            ushort jstfScriptOffset = reader.ReadUShort();
            reader.Seek(jstfScriptOffset);
            JstfScript = new JstfScript(reader);
        }
    }
}