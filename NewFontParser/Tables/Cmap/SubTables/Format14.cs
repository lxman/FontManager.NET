﻿using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format14 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; } = -1;

        public uint NumVarSelectorRecords { get; }

        public List<VariationSelectorRecord> VarSelectorRecords { get; } = new List<VariationSelectorRecord>();

        public Format14(BigEndianReader reader)
        {
            Format = reader.ReadUInt16();
            Length = reader.ReadUInt32();
            NumVarSelectorRecords = reader.ReadUInt32();
            for (var i = 0; i < NumVarSelectorRecords; i++)
            {
                VarSelectorRecords.Add(new VariationSelectorRecord(reader));
            }
        }
    }
}