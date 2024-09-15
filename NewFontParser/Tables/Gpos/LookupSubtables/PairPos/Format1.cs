﻿using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class Format1 : ILookupSubTable
    {
        public ushort PosFormat { get; }

        public ValueFormat ValueFormat1 { get; }

        public ValueFormat ValueFormat2 { get; }

        public PairSet[] PairSets { get; }

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            PosFormat = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ValueFormat1 = (ValueFormat)reader.ReadShort();
            ValueFormat2 = (ValueFormat)reader.ReadShort();
            ushort pairSetCount = reader.ReadUShort();
            ushort[] pairSetOffsets = reader.ReadUShortArray(pairSetCount);
            PairSets = new PairSet[pairSetCount];
            for (var i = 0; i < pairSetCount; i++)
            {
                reader.Seek(startOfTable + pairSetOffsets[i]);
                PairSets[i] = new PairSet(reader, new List<ValueFormat> { ValueFormat1, ValueFormat2 });
            }
        }
    }
}