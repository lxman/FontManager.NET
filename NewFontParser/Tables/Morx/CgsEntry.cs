using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx
{
    public class CgsEntry
    {
        public ushort NewState { get; }

        public ActionFlags Flags { get; }

        public ushort MarkIndex { get; }

        public ushort CurrentIndex { get; }

        public CgsEntry(BigEndianReader reader)
        {
            NewState = reader.ReadUShort();
            Flags = (ActionFlags)reader.ReadUShort();
            MarkIndex = reader.ReadUShort();
            CurrentIndex = reader.ReadUShort();
        }
    }
}
