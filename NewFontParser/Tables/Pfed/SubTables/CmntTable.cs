﻿using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Pfed.SubTables
{
    public class CmntTable : IPfedSubtable
    {
        public ushort Version { get; }

        public List<CmntItem> Items { get; } = new List<CmntItem>();

        public CmntTable(BigEndianReader reader)
        {
            Version = reader.ReadUShort();
            ushort numItems = reader.ReadUShort();
            for (var i = 0; i < numItems; i++)
            {
                Items.Add(new CmntItem(reader, Version));
            }
        }
    }
}