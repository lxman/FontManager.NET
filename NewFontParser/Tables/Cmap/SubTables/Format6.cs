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
            Format = reader.ReadUint16();
            Length = reader.ReadUint16();
            Language = reader.ReadInt16();
            FirstCode = reader.ReadUint16();
            EntryCount = reader.ReadUint16();
            GlyphIndexArray = new List<uint>();
            for (var i = 0; i < EntryCount; i++)
            {
                GlyphIndexArray.Add(reader.ReadUint16());
            }
        }
    }
}