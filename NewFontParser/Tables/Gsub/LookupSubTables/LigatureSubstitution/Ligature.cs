﻿using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gsub.LookupSubTables.LigatureSubstitution
{
    public class Ligature
    {
        public ushort LigatureGlyph { get; }

        public ushort[] ComponentGlyphIds { get; }

        public Ligature(BigEndianReader reader)
        {
            LigatureGlyph = reader.ReadUShort();
            ushort componentCount = reader.ReadUShort();
            ComponentGlyphIds = reader.ReadUShortArray(Convert.ToUInt32(componentCount - 1));
        }
    }
}