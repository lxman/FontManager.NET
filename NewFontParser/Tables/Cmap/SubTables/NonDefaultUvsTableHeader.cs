using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class NonDefaultUvsTableHeader
    {
        public uint NumUvsMappings { get; }

        public List<UvsMappingRecord> UvsMappings { get; } = new List<UvsMappingRecord>();

        public NonDefaultUvsTableHeader(BigEndianReader reader)
        {
            NumUvsMappings = reader.ReadUInt32();
            for (var i = 0; i < NumUvsMappings; i++)
            {
                UvsMappings.Add(new UvsMappingRecord(reader.ReadBytes(SequentialMapGroup.RecordSize)));
            }
        }
    }
}