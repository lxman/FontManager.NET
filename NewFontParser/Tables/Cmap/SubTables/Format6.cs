using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format6 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public uint FirstCode { get; }

        public uint EntryCount { get; }

        public List<uint> GlyphIndexArray { get; }

        public Format6(BigEndianReader reader)
        {
            Format = reader.ReadUInt16();
            Length = reader.ReadUInt16();
            Language = reader.ReadInt16();
            FirstCode = reader.ReadUInt16();
            EntryCount = reader.ReadUInt16();
            GlyphIndexArray = new List<uint>();
            for (var i = 0; i < EntryCount; i++)
            {
                GlyphIndexArray.Add(reader.ReadUInt16());
            }
        }
    }
}