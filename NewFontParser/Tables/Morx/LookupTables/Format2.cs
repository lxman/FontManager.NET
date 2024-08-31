﻿using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class Format2 : IFsHeader
    {
        public BinarySearchHeader Header { get; }

        public List<LookupSegment2> Segments { get; } = new List<LookupSegment2>();

        public Format2(BigEndianReader reader)
        {
            Header = new BinarySearchHeader(reader);
            for (var i = 0; i < Header.NUnits; i++)
            {
                Segments.Add(new LookupSegment2(reader, Header.UnitSize));
            }
        }
    }
}
