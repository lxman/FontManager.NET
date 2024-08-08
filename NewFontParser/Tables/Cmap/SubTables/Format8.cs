﻿using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format8 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public uint Is32 { get; }

        public uint NumGroups { get; }

        public List<SequentialMapGroup> SequentialMapGroups { get; } = new List<SequentialMapGroup>();

        public Format8(BigEndianReader reader)
        {
            Format = reader.ReadUint16();
            _ = reader.ReadUint16();
            Length = reader.ReadUint32();
            Language = reader.ReadInt32();
            Is32 = reader.ReadByte();
            NumGroups = reader.ReadUint32();
            for (var i = 0; i < NumGroups; i++)
            {
                SequentialMapGroups.Add(new SequentialMapGroup(reader.ReadBytes(SequentialMapGroup.RecordSize)));
            }
        }
    }
}