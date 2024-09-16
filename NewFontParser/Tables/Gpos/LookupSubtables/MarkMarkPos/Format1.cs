﻿using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Gpos.LookupSubtables.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat MarkCoverage { get; }

        public ICoverageFormat Mark2Coverage { get; }

        public MarkArray MarkArray { get; }

        public Mark2Array Mark2Array { get; }

        public Format1(BigEndianReader reader)
        {
            long tableBase = reader.Position;

            Format = reader.ReadUShort();
            ushort mark1CoverageOffset = reader.ReadUShort();
            ushort mark2CoverageOffset = reader.ReadUShort();
            ushort markClassCount = reader.ReadUShort();
            ushort markArrayOffset = reader.ReadUShort();
            ushort mark2ArrayOffset = reader.ReadUShort();

            reader.Seek(tableBase + mark1CoverageOffset);
            MarkCoverage = CoverageTable.Retrieve(reader);
            reader.Seek(tableBase + mark2CoverageOffset);
            Mark2Coverage = CoverageTable.Retrieve(reader);
            reader.Seek(tableBase + markArrayOffset);
            MarkArray = new MarkArray(reader);
            reader.Seek(tableBase + mark2ArrayOffset);
            Mark2Array = new Mark2Array(reader, markClassCount);
        }
    }
}