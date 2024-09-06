﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Bdat.GlyphBitmap
{
    public class Format4 : IGlyphBitmap
    {
        public uint WhiteTreeOffset { get; }

        public uint BlackTreeOffset { get; }

        public uint GlyphDataOffset { get; }

        public Format4(BigEndianReader reader)
        {
            WhiteTreeOffset = reader.ReadUInt32();
            BlackTreeOffset = reader.ReadUInt32();
            GlyphDataOffset = reader.ReadUInt32();
        }
    }
}