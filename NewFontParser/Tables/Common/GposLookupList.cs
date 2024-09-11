using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class GposLookupList
    {
        public List<GposLookupTable> LookupTables { get; } = new List<GposLookupTable>();

        public GposLookupList(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort lookupCount = reader.ReadUShort();
            ushort[] lookups = reader.ReadUShortArray(lookupCount);

            for (var i = 0; i < lookupCount; i++)
            {
                reader.Seek(lookups[i] + position);
                LookupTables.Add(new GposLookupTable(reader));
            }
        }
    }
}