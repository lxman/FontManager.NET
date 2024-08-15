using System;
using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetZonePointer2 : IInstruction
    {
        public byte OpCode => 0x15;
        public string Mnemonic => "SZP2[]";
        public string Description => "Set zone pointer 2.";

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
            graphicsState.ZonePointers[2] = Convert.ToBoolean(stack.Pop());
        }
    }
}
