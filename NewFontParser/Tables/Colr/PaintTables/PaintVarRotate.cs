﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarRotate : IPaintTable
    {
        public byte Format => 25;

        public float Angle { get; }

        public uint VarIndexBase { get; }

        public IPaintTable SubTable { get; }

        public PaintVarRotate(BigEndianReader reader)
        {
            uint subTableOffset = reader.ReadUInt24();
            Angle = reader.ReadF2Dot14();
            VarIndexBase = reader.ReadUInt32();
            SubTable = PaintTableFactory.CreatePaintTable(reader, subTableOffset);
        }
    }
}