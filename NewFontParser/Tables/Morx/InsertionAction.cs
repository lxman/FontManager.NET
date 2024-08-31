using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx
{
    public class InsertionAction
    {
        public ushort NewState { get; }

        public ActionFlags Flags { get; }

        public ushort CurrentInsertIndex { get; }

        public ushort MarkedInsertIndex { get; }

        public InsertionAction(BigEndianReader reader)
        {
            NewState = reader.ReadUShort();
            Flags = (ActionFlags)reader.ReadUShort();
            CurrentInsertIndex = reader.ReadUShort();
            MarkedInsertIndex = reader.ReadUShort();
        }
    }
}
