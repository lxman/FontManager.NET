using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class ReadStore : IInstruction
    {
        public byte OpCode => 0x43;

        public string Mnemonic => "RS[]";

        public string Description => "Read from storage and push on stack";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            byte index = reader.ReadByte();
            stack.Push(storageArea[index]);
        }
    }
}
