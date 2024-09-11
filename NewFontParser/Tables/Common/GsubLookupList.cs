using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class GsubLookupList
    {
        public List<GsubLookupTable> LookupTables { get; } = new List<GsubLookupTable>();

        public GsubLookupList(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort lookupCount = reader.ReadUShort();
            ushort[] lookups = reader.ReadUShortArray(lookupCount);

            for (var i = 0; i < lookupCount; i++)
            {
                reader.Seek(lookups[i] + position);
                LookupTables.Add(new GsubLookupTable(reader));
            }
        }
    }
}