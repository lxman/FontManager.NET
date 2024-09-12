﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Todo.Graphite.Feat
{
    public class Settings
    {
        public ushort Value { get; }

        public ushort NameId { get; }

        public Settings(BigEndianReader reader)
        {
            Value = reader.ReadUShort();
            NameId = reader.ReadUShort();
        }
    }
}