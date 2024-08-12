using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format2 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public List<ushort> SubHeaderKeys { get; } = new List<ushort>();

        public List<Format2SubHeader> SubHeaders { get; } = new List<Format2SubHeader>();

        public List<ushort> GlyphIndexArray { get; } = new List<ushort>();

        public Format2(BigEndianReader reader)
        {
            Format = reader.ReadUint16();
            Length = reader.ReadUint16();
            _ = reader.ReadUint16();
            Language = reader.ReadInt16();
            for (var i = 0; i < 256; i++)
            {
                SubHeaderKeys.Add((byte)(reader.ReadUShort() >> 8));
            }
            foreach (ushort key in SubHeaderKeys)
            {
                reader.Seek(key);
                SubHeaders.Add(new Format2SubHeader(reader.ReadBytes(Format2SubHeader.RecordSize)));
            }
            foreach (Format2SubHeader subHeader in SubHeaders)
            {
                for (var i = 0; i < subHeader.EntryCount; i++)
                {
                    GlyphIndexArray.Add(reader.ReadUShort());
                }
            }
        }
    }
}