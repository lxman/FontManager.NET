using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter
{
    public interface IInstruction
    {
        byte OpCode { get; }

        string Mnemonic { get; }

        string Description { get; }

        void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack);
    }
}
