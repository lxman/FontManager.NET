using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format0 : ICmapSubtable
    {
        public static long RecordSize => 262;

        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public List<uint> GlyphIndexArray { get; } = new List<uint>();

        public Format0(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            Length = reader.ReadUShort();
            Language = reader.ReadInt16();
            for (var i = 0; i < 256; i++)
            {
                GlyphIndexArray.Add(reader.ReadByte());
            }
        }
    }
}