using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.CursivePos
{
    public class EntryExitRecord
    {
        public ushort EntryAnchorOffset { get; }

        public ushort ExitAnchorOffset { get; }

        public EntryExitRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);

            EntryAnchorOffset = reader.ReadUShort();
            ExitAnchorOffset = reader.ReadUShort();
        }
    }
}