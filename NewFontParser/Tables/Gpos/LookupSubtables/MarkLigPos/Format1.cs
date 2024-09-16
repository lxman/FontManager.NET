﻿using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Gpos.LookupSubtables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat MarkCoverage { get; }

        public ICoverageFormat LigatureCoverage { get; }

        public MarkArray MarkArray { get; }

        public LigatureArrayTable? LigatureArrayTable { get; }

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;

            Format = reader.ReadUShort();
            ushort markCoverageOffset = reader.ReadUShort();
            ushort ligatureCoverageOffset = reader.ReadUShort();
            ushort markClassCount = reader.ReadUShort();
            ushort markArrayOffset = reader.ReadUShort();
            ushort ligatureArrayOffset = reader.ReadUShort();

            if (ligatureArrayOffset == 0) return;
            reader.Seek(startOfTable + ligatureArrayOffset);
            LigatureArrayTable = new LigatureArrayTable(reader, markClassCount);
            reader.Seek(startOfTable + markCoverageOffset);
            MarkCoverage = CoverageTable.Retrieve(reader);
            reader.Seek(startOfTable + ligatureCoverageOffset);
            LigatureCoverage = CoverageTable.Retrieve(reader);
            reader.Seek(startOfTable + markArrayOffset);
            MarkArray = new MarkArray(reader);
        }
    }
}