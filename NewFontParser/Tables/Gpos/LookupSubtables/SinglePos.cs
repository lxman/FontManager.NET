using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables
{
    public class SinglePos : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort CoverageOffset { get; }

        public ValueFormat ValueFormat { get; }

        public ValueRecord? ValueRecord { get; }

        public ushort? ValueCount { get; }

        public List<ValueRecord>? ValueRecords { get; }

        public SinglePos(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ValueFormat = (ValueFormat)reader.ReadUShort();
            if (Format == 1)
            {
                ValueRecord = new ValueRecord(ValueFormat, reader);
                return;
            }
            ValueCount = reader.ReadUShort();
            ValueRecords = new List<ValueRecord>();
            for (var i = 0; i < ValueCount; i++)
            {
                ValueRecords.Add(new ValueRecord(ValueFormat, reader));
            }
        }
    }
}