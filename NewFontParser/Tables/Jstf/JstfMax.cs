using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfMax
    {
        public List<ushort> LookupOffsets { get; } = new List<ushort>();

        public JstfMax(BigEndianReader reader)
        {
            ushort lookupCount = reader.ReadUShort();
            for (var i = 0; i < lookupCount; i++)
            {
                LookupOffsets.Add(reader.ReadUShort());
            }
        }
    }
}