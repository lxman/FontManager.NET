﻿using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format12 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public uint NumGroups { get; }

        public List<SequentialMapGroup> Groups { get; } = new List<SequentialMapGroup>();

        public Format12(BigEndianReader reader)
        {
            Format = reader.ReadUint16();
            _ = reader.ReadUint16();
            Length = reader.ReadUint32();
            Language = reader.ReadInt32();
            NumGroups = reader.ReadUint32();
            for (var i = 0; i < NumGroups; i++)
            {
                Groups.Add(new SequentialMapGroup(reader.ReadBytes(SequentialMapGroup.RecordSize)));
            }
        }
    }
}