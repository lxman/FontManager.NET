﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.SequenceContext.Format1
{
    public class SequenceRule
    {
        public ushort GlyphCount { get; }

        public ushort[] GlyphIds { get; }

        public SequenceRule(BigEndianReader reader)
        {
            GlyphCount = reader.ReadUShort();
            GlyphIds = new ushort[GlyphCount];

            for (var i = 0; i < GlyphCount; i++)
            {
                GlyphIds[i] = reader.ReadUShort();
            }
        }
    }
}