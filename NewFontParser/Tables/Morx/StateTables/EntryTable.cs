using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.StateTables
{
    public class EntryTable
    {
        public ushort NewState { get; }

        public ushort Flags { get; }

        public List<uint> GlyphOffsets { get; } = new List<uint>();

        public EntryTable(BigEndianReader reader)
        {
            NewState = reader.ReadUShort();
            Flags = reader.ReadUShort();
        }
    }
}