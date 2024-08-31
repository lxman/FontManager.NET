﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarScale : IPaintTable
    {
        public byte Format => 17;

        public IPaintTable SubTable { get; }

        public float ScaleX { get; }

        public float ScaleY { get; }

        public uint VarIndexBase { get; }

        public PaintVarScale(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            ScaleX = reader.ReadF2Dot14();
            ScaleY = reader.ReadF2Dot14();
            VarIndexBase = reader.ReadUInt32();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}