using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace NewFontParser.Tables.Gsub.LookupSubTables.MultipleSubstitution
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat Coverage { get; }

        public List<SequenceTable> Sequences { get; } = new List<SequenceTable>();

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort sequenceCount = reader.ReadUShort();
            ushort[] sequenceOffsets = reader.ReadUShortArray(sequenceCount);
            for (var i = 0; i < sequenceCount; i++)
            {
                reader.Seek(startOfTable + sequenceOffsets[i]);
                Sequences.Add(new SequenceTable(reader));
            }
            reader.Seek(startOfTable + coverageOffset);
            byte format = reader.PeekBytes(2)[1];
            Coverage = format switch
            {
                1 => new Common.CoverageFormat.Format1(reader),
                2 => new Common.CoverageFormat.Format2(reader),
                _ => Coverage
            };
        }
    }
}
