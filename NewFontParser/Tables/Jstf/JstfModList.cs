using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfModList
    {
        public List<ushort> GsubLookupIndices { get; } = new List<ushort>();

        public JstfModList(BigEndianReader reader)
        {
            ushort lookupCount = reader.ReadUShort();
            for (var i = 0; i < lookupCount; i++)
            {
                GsubLookupIndices.Add(reader.ReadUShort());
            }
        }
    }
}