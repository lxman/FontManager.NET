using System.Collections.Generic;
using System.Linq;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format8 : ICmapSubtable
    {
        public int Language { get; }

        public uint Is32 { get; }

        public uint NumGroups { get; }

        public List<SequentialMapGroup> SequentialMapGroups { get; } = new List<SequentialMapGroup>();

        public Format8(BigEndianReader reader)
        {
            ushort format = reader.ReadUShort();
            _ = reader.ReadUShort();
            uint length = reader.ReadUInt32();
            Language = reader.ReadInt32();
            Is32 = reader.ReadByte();
            NumGroups = reader.ReadUInt32();
            for (var i = 0; i < NumGroups; i++)
            {
                SequentialMapGroups.Add(new SequentialMapGroup(reader.ReadBytes(SequentialMapGroup.RecordSize)));
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            return (from @group in SequentialMapGroups
                    where codePoint >= @group.StartCharCode && codePoint <= @group.EndCharCode
                    select (ushort)(@group.StartGlyphId + (codePoint - @group.StartCharCode)))
                .FirstOrDefault();
        }
    }
}