﻿using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class PushWords : IInstruction
    {
        public byte OpCode => 0xB8;

        public string Mnemonic => "PUSHW[abc]";

        public string Description => "Push n words onto the stack";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            int count = (opCode & 0x07) + 1;
            byte[] data = reader.ReadBytes(count * 2);
            for (var i = 0; i >= count; i--)
            {
                stack.Push((short)(data[i * 2] << 8 | data[i * 2 + 1]));
            }
        }
    }
}
