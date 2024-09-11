using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class GsubLookupTable
    {
        public GsubLookupType LookupType { get; }

        public LookupFlag LookupFlags { get; }

        public ushort SubTableCount { get; }

        public ushort[] SubTableOffsets { get; }

        public ushort? MarkFilteringSet { get; }

        public List<GsubLookupTable> SubTables { get; } = new List<GsubLookupTable>();

        public GsubLookupTable(BigEndianReader reader)
        {
            long position = reader.Position;
            LookupType = (GsubLookupType)reader.ReadUShort();
            LookupFlags = (LookupFlag)reader.ReadUShort();
            SubTableCount = reader.ReadUShort();
            SubTableOffsets = new ushort[SubTableCount];
            for (var i = 0; i < SubTableCount; i++)
            {
                SubTableOffsets[i] = reader.ReadUShort();
            }
            if (LookupFlags.HasFlag(LookupFlag.UseMarkFilteringSet))
            {
                MarkFilteringSet = reader.ReadUShort();
            }
            for (var i = 0; i < SubTableCount; i++)
            {
                reader.Seek(SubTableOffsets[i] + position);
                // TODO: Come back and fix this
                //SubTables.Add(new GsubLookupTable(reader));
            }
        }
    }
}