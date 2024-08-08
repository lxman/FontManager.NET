using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format10 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public uint StartChar { get; }

        public uint NumChars { get; }

        public List<uint> GlyphIndexArray { get; } = new List<uint>();

        public Format10(BigEndianReader reader)
        {
            Format = reader.ReadUint16();
            _ = reader.ReadUint16();
            Length = reader.ReadUint32();
            Language = reader.ReadInt32();
            StartChar = reader.ReadUint32();
            NumChars = reader.ReadUint32();
            for (var i = 0; i < NumChars; i++)
            {
                GlyphIndexArray.Add(reader.ReadUint16());
            }
        }
    }
}