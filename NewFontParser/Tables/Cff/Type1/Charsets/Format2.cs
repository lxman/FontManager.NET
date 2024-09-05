using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1.Charsets
{
    public class Format2 : ICharset
    {
        public List<Range2> Ranges { get; } = new List<Range2>();

        public Format2(BigEndianReader reader, ushort numGlyphs)
        {
            for (var i = 0; i < numGlyphs; i++)
            {
                Ranges.Add(new Range2(reader));
            }
        }
    }
}