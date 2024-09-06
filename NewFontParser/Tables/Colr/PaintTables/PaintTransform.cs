﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintTransform : IPaintTable
    {
        public byte Format => 12;

        public IPaintTable SubTable { get; }

        public Affine2X3 Transform { get; }

        public PaintTransform(BigEndianReader reader)
        {
            long start = reader.Position;
            uint paintOffset = reader.ReadUInt24();
            uint transformOffset = reader.ReadUInt24();
            long beforeBuilding = reader.Position;
            SubTable = PaintTableFactory.CreatePaintTable(reader, start + paintOffset);
            reader.Seek(start + transformOffset);
            Transform = new Affine2X3(reader);
            reader.Seek(beforeBuilding);
        }
    }
}