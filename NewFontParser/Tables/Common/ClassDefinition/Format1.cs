using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ClassDefinition
{
    public class Format1 : IClassDefinition
    {
        public ushort Format { get; }

        public ushort StartGlyph { get; }

        public List<ushort> ClassValues { get; } = new List<ushort>();

        public Format1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            StartGlyph = reader.ReadUShort();
            var glyphCount = reader.ReadUShort();
            for (var i = 0; i < glyphCount; i++)
            {
                ClassValues.Add(reader.ReadUShort());
            }
        }
    }
}