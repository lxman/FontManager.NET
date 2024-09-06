using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace NewFontParser.Tables.Math
{
    public class MathKernInfoTable
    {
        public ICoverageFormat MathKernCoverage { get; }

        public List<MathKernInfoRecord> MathKernInfoRecords { get; } = new List<MathKernInfoRecord>();

        public MathKernInfoTable(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort mathKernCoverageOffset = reader.ReadUShort();

            ushort mathKernCount = reader.ReadUShort();

            ushort[] mathKernValues = reader.ReadUShortArray(4 * mathKernCount);
            ushort mathKernIndex = 0;

            for (var i = 0; i < mathKernCount; i++)
            {
                MathKernInfoRecords.Add(new MathKernInfoRecord(reader, position, mathKernValues[mathKernIndex..(mathKernIndex + 4)]));
                mathKernIndex += 4;
            }

            reader.Seek(position + mathKernCoverageOffset);

            byte format = reader.PeekBytes(2)[1];
            MathKernCoverage = format switch
            {
                1 => new Format1(reader),
                2 => new Format2(reader),
                _ => MathKernCoverage
            };
        }
    }
}