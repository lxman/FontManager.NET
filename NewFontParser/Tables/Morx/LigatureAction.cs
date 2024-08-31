﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx
{
    public class LigatureAction
    {
        public ushort NextStateIndex { get; }

        public EntryFlags Flags { get; }

        public ushort LigActionIndex { get; }

        public LigatureAction(BigEndianReader reader)
        {
            NextStateIndex = reader.ReadUShort();
            Flags = (EntryFlags)reader.ReadUShort();
            LigActionIndex = reader.ReadUShort();
        }
    }
}
