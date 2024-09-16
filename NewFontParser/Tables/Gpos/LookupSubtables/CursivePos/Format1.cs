using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gpos.LookupSubtables.CursivePos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat Coverage { get; }

        public List<EntryExitRecord> EntryExitRecords { get; } = new List<EntryExitRecord>();

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort entryExitCount = reader.ReadUShort();
            for (var i = 0; i < entryExitCount; i++)
            {
                EntryExitRecords.Add(new EntryExitRecord(reader.ReadBytes(4)));
            }
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}