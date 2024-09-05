﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarScaleUniformAroundCenter : IPaintTable
    {
        public byte Format => 23;

        public IPaintTable SubTable { get; }

        public float Scale { get; }

        public short CenterX { get; }

        public short CenterY { get; }

        public uint VarIndexBase { get; }

        public PaintVarScaleUniformAroundCenter(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            Scale = reader.ReadF2Dot14();
            CenterX = reader.ReadShort();
            CenterY = reader.ReadShort();
            VarIndexBase = reader.ReadUInt32();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}