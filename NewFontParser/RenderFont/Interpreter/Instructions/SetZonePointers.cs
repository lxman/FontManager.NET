using System;
using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetZonePointers : IInstruction
    {
        public byte OpCode => 0x17;
        public string Mnemonic => "SZPS[]";
        public string Description => "Set Zone Pointers";

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
            var setValue = Convert.ToBoolean(stack.Pop());
            graphicsState.ZonePointers[0] = setValue;
            graphicsState.ZonePointers[1] = setValue;
            graphicsState.ZonePointers[2] = setValue;
        }
    }
}
