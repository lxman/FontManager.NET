using NewFontParser.Reader;
using NewFontParser.Tables.Morx.StateTables;

namespace NewFontParser.Tables.Morx
{
    public class GlyphInsertionSubtable
    {
        public MorxStateTable StateTable { get; }

        public InsertionAction InsertionAction { get; }

        public GlyphInsertionSubtable(BigEndianReader reader)
        {
            StateTable = new MorxStateTable(reader);
            ushort insertionActionOffset = reader.ReadUShort();
            reader.Seek(insertionActionOffset);
            InsertionAction = new InsertionAction(reader);
        }
    }
}