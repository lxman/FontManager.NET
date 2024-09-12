using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format6 : ICmapSubtable
    {
        public int Language { get; }

        public uint FirstCode { get; }

        public uint EntryCount { get; }

        public List<uint> GlyphIndexArray { get; }

        public Format6(BigEndianReader reader)
        {
            ushort format = reader.ReadUShort();
            uint length = reader.ReadUShort();
            Language = reader.ReadInt16();
            FirstCode = reader.ReadUShort();
            EntryCount = reader.ReadUShort();
            GlyphIndexArray = new List<uint>();
            for (var i = 0; i < EntryCount; i++)
            {
                GlyphIndexArray.Add(reader.ReadUShort());
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            if (codePoint < FirstCode || codePoint >= FirstCode + EntryCount)
            {
                return 0; // Code point is out of range
            }

            var index = (int)(codePoint - FirstCode);
            return (ushort)GlyphIndexArray[index];
        }
    }
}