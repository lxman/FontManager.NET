﻿using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort MarkCoverageOffset { get; }

        public ushort LigatureCoverageOffset { get; }

        public ushort MarkClassCount { get; }

        public ushort MarkArrayOffset { get; }

        public ushort LigatureArrayOffset { get; }

        public LigatureArrayTable? LigatureArrayTable { get; }

        public Format1(BigEndianReader reader)
        {
            long position = reader.Position;

            Format = reader.ReadUShort();
            MarkCoverageOffset = reader.ReadUShort();
            LigatureCoverageOffset = reader.ReadUShort();
            MarkClassCount = reader.ReadUShort();
            MarkArrayOffset = reader.ReadUShort();
            LigatureArrayOffset = reader.ReadUShort();

            if (LigatureArrayOffset == 0) return;
            reader.Seek(position + LigatureArrayOffset);
            LigatureArrayTable = new LigatureArrayTable(reader, MarkClassCount);
        }
    }
}