using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class WriteStore : IInstruction
    {
        public byte OpCode => 0x42;

        public string Mnemonic => "WS[]";

        public string Description => "Write the value from the stack to the storage area.";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            int value = stack.Pop();
            int location = stack.Pop();
            storageArea[location] = value;
        }
    }
}
