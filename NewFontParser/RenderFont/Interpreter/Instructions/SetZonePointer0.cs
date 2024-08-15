using System;
using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetZonePointer0 : IInstruction
    {
        public byte OpCode => 0x13;
        public string Mnemonic => "SZP0[]";
        public string Description => "Set zone pointer 0.";

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
            graphicsState.ZonePointers[0] = Convert.ToBoolean(stack.Pop());
        }
    }
}
