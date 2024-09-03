using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format13 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public int NumGroups { get; }

        public List<ConstantMapGroup> Groups { get; } = new List<ConstantMapGroup>();

        public Format13(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            _ = reader.ReadUShort();
            Length = reader.ReadUInt32();
            Language = reader.ReadInt32();
            NumGroups = reader.ReadInt32();
            for (var i = 0; i < NumGroups; i++)
            {
                Groups.Add(new ConstantMapGroup(reader.ReadBytes(ConstantMapGroup.RecordSize)));
            }
        }
    }
}