using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format10 : ICmapSubtable
    {
        public int Language { get; }

        public uint StartChar { get; }

        public uint NumChars { get; }

        public List<uint> GlyphIndexArray { get; } = new List<uint>();

        public Format10(BigEndianReader reader)
        {
            ushort format = reader.ReadUShort();
            _ = reader.ReadUShort();
            uint length = reader.ReadUInt32();
            Language = reader.ReadInt32();
            StartChar = reader.ReadUInt32();
            NumChars = reader.ReadUInt32();
            for (var i = 0; i < NumChars; i++)
            {
                GlyphIndexArray.Add(reader.ReadUShort());
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            uint index = codePoint - StartChar;
            if (index < NumChars)
            {
                return (ushort)GlyphIndexArray[(int)index];
            }
            return 0;
        }
    }
}