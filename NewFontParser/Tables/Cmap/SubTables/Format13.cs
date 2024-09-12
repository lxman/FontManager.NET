using System.Collections.Generic;
using System.Linq;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format13 : ICmapSubtable
    {
        public int Language { get; }

        public int NumGroups { get; }

        public List<ConstantMapGroup> Groups { get; } = new List<ConstantMapGroup>();

        public Format13(BigEndianReader reader)
        {
            ushort format = reader.ReadUShort();
            _ = reader.ReadUShort();
            uint length = reader.ReadUInt32();
            Language = reader.ReadInt32();
            NumGroups = reader.ReadInt32();
            for (var i = 0; i < NumGroups; i++)
            {
                Groups.Add(new ConstantMapGroup(reader.ReadBytes(ConstantMapGroup.RecordSize)));
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            return (from @group in Groups
                    where codePoint >= @group.StartCharCode && codePoint <= @group.EndCharCode
                    select (ushort)@group.GlyphId)
                .FirstOrDefault();
        }
    }
}