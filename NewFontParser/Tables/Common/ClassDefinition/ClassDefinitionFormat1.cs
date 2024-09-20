using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ClassDefinition
{
    public class ClassDefinitionFormat1 : IClassDefinition
    {
        public ushort Format { get; }

        public ushort StartGlyph { get; }

        public List<ushort> ClassValues { get; } = new List<ushort>();

        public ClassDefinitionFormat1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            StartGlyph = reader.ReadUShort();
            ushort glyphCount = reader.ReadUShort();
            for (var i = 0; i < glyphCount; i++)
            {
                ClassValues.Add(reader.ReadUShort());
            }
        }
    }
}