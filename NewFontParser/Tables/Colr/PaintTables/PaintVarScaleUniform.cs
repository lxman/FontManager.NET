﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr.PaintTables
{
    public class PaintVarScaleUniform : IPaintTable
    {
        public byte Format => 21;

        public IPaintTable SubTable { get; }

        public float Scale { get; }

        public uint VarIndexBase { get; }

        public PaintVarScaleUniform(BigEndianReader reader)
        {
            uint paintOffset = reader.ReadUInt24();
            Scale = reader.ReadF2Dot14();
            VarIndexBase = reader.ReadUInt32();
            SubTable = PaintTableFactory.CreatePaintTable(reader, paintOffset);
        }
    }
}
