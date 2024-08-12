using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class PushNBytes : IInstruction
    {
        public byte OpCode { get; } = 0x40;

        public string Mnemonic { get; } = "NPUSHB[]";

        public string Description { get; } = "Push N bytes from the instruction stream to the stack";

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
                stack.Push(reader.ReadByte());
            }
        }
    }
}