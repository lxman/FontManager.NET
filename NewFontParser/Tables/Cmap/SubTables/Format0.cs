using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format0 : ICmapSubtable
    {
        public int Language { get; }

        public List<uint> GlyphIndexArray { get; } = new List<uint>();

        public Format0(BigEndianReader reader)
        {
            ushort format = reader.ReadUShort();
            ushort length = reader.ReadUShort();
            Language = reader.ReadInt16();
            for (var i = 0; i < 256; i++)
            {
                GlyphIndexArray.Add(reader.ReadByte());
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            return codePoint < 256 ? (ushort)GlyphIndexArray[codePoint] : (ushort)0;
        }
    }
}