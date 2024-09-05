using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class GsubLookupList
    {
        public ushort[] Lookups { get; }

        public List<GsubLookupTable> LookupTables { get; } = new List<GsubLookupTable>();

        public GsubLookupList(BigEndianReader reader, ushort lookupListOffset)
        {
            reader.Seek(lookupListOffset);
            long position = reader.Position;

            ushort lookupCount = reader.ReadUShort();
            Lookups = new ushort[lookupCount];
            for (var i = 0; i < lookupCount; i++)
            {
                Lookups[i] = reader.ReadUShort();
            }

            for (var i = 0; i < lookupCount; i++)
            {
                reader.Seek(Lookups[i] + position);
                LookupTables.Add(new GsubLookupTable(reader));
            }
        }
    }
}