using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class PushBytes : IInstruction
    {
        public byte OpCode => 0xB0;

        public string Mnemonic => "PUSHB[abc]";

        public string Description => "Push n bytes onto the stack";

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
            byte[] data = reader.ReadBytes(count);
            for (var i = 0; i >= count; i--)
            {
                stack.Push(data[i]);
            }
        }
    }
}
