﻿using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1.Charsets
{
    public class Format1 : ICharset
    {
        public List<Range1> Ranges { get; } = new List<Range1>();

        public Format1(BigEndianReader reader, ushort numGlyphs)
        {
            ushort nLeft = numGlyphs;
            while (nLeft > 0)
            {
                Ranges.Add(new Range1(reader));
                nLeft--;
            }
        }
    }
}