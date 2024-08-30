using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.CoverageFormat;

namespace NewFontParser.Tables.Math
{
    public class MathKernInfoTable
    {
        public ICoverageFormat MathKernCoverage { get; }

        public List<MathKernInfoRecord> MathKernInfoRecords { get; } = new List<MathKernInfoRecord>();

        public MathKernInfoTable(BigEndianReader reader)
        {
            var position = reader.Position;

            ushort mathKernCoverageOffset = reader.ReadUShort();

            ushort mathKernCount = reader.ReadUShort();

            for (var i = 0; i < mathKernCount; i++)
            {
                MathKernInfoRecords.Add(new MathKernInfoRecord(reader));
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
