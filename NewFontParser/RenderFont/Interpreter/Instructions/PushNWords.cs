using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class PushNWords : IInstruction
    {
        public byte OpCode => 0x41;

        public string Mnemonic => "NPUSHW[]";

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
            byte n = reader.ReadByte();
            for (var i = 0; i < n; i++)
            {
                stack.Push(reader.ReadWord());
            }
        }
    }
}
