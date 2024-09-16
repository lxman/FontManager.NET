using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class GsubLookupList
    {
        public List<GsubLookupTable> LookupTables { get; } = new List<GsubLookupTable>();

        public GsubLookupList(BigEndianReader reader)
        {
            long startOfTable = reader.Position;

            ushort lookupCount = reader.ReadUShort();
            ushort[] lookupOffsets = reader.ReadUShortArray(lookupCount);

            for (var i = 0; i < lookupCount; i++)
            {
                reader.Seek(startOfTable + lookupOffsets[i]);
                LookupTables.Add(new GsubLookupTable(reader));
            }
        }
    }
}