using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class ExtenderGlyph
    {
        public List<ushort> ExtenderGlyphs { get; } = new List<ushort>();

        public ExtenderGlyph(BigEndianReader reader)
        {
            ushort glyphCount = reader.ReadUShort();
            for (var i = 0; i < glyphCount; i++)
            {
                ExtenderGlyphs.Add(reader.ReadUShort());
            }
        }
    }
}