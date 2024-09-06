using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Merg
{
    public class MergeEntryRow
    {
        public List<MergeEntryFlags> MergeEntries { get; } = new List<MergeEntryFlags>();

        public MergeEntryRow(BigEndianReader reader, ushort mergeClassCount)
        {
            for (var i = 0; i < mergeClassCount; i++)
            {
                MergeEntries.Add((MergeEntryFlags)reader.ReadByte());
            }
        }
    }
}