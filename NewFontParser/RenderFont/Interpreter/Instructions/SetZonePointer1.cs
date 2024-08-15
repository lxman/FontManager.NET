using System;
using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetZonePointer1 : IInstruction
    {
        public byte OpCode => 0x14;
        public string Mnemonic => "SZP1[]";
        public string Description => "Set zone pointer 1.";

        public void Execute
        (
            byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack
        )
        {
            graphicsState.ZonePointers[1] = Convert.ToBoolean(stack.Pop());
        }
    }
}
