using System.Collections.Generic;
using System.Linq;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format12 : ICmapSubtable
    {
        public int Language { get; }

        public uint NumGroups { get; }

        public List<SequentialMapGroup> Groups { get; } = new List<SequentialMapGroup>();

        public Format12(BigEndianReader reader)
        {
            ushort format = reader.ReadUShort();
            _ = reader.ReadUShort();
            uint length = reader.ReadUInt32();
            Language = reader.ReadInt32();
            NumGroups = reader.ReadUInt32();
            for (var i = 0; i < NumGroups; i++)
            {
                Groups.Add(new SequentialMapGroup(reader.ReadBytes(SequentialMapGroup.RecordSize)));
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            return (from @group in Groups
                    where codePoint >= @group.StartCharCode && codePoint <= @group.EndCharCode
                    select (ushort)(@group.StartGlyphId + (codePoint - @group.StartCharCode)))
                .FirstOrDefault();
        }
    }
}