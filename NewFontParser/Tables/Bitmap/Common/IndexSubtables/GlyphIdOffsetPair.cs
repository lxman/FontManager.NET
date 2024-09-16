﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.IndexSubtables
{
    public class GlyphIdOffsetPair
    {
        public ushort GlyphId { get; }

        public ushort Offset { get; }

        public GlyphIdOffsetPair(BigEndianReader reader)
        {
            GlyphId = reader.ReadUShort();
            Offset = reader.ReadUShort();
        }
    }
}