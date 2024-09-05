using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class GposLookupList
    {
        public ushort[] Lookups { get; }

        public List<GposLookupTable> LookupTables { get; } = new List<GposLookupTable>();

        public GposLookupList(BigEndianReader reader, ushort lookupListOffset)
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
                LookupTables.Add(new GposLookupTable(reader));
            }
        }
    }
}