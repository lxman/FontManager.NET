using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.CursivePos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort CoverageOffset { get; }

        public ushort EntryExitCount { get; }

        public List<EntryExitRecord> EntryExitRecords { get; } = new List<EntryExitRecord>();

        public Format1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            EntryExitCount = reader.ReadUShort();
            for (var i = 0; i < EntryExitCount; i++)
            {
                EntryExitRecords.Add(new EntryExitRecord(reader.ReadBytes(4)));
            }
        }
    }
}