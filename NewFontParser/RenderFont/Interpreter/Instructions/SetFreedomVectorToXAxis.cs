using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetFreedomVectorToXAxis : IInstruction
    {
        public byte OpCode => 0x05;

        public string Mnemonic => "SFVTCA[X]";

        public string Description => "Set freedom vector to X-axis";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            graphicsState.FreedomVector = new PointF(1, 0);
        }
    }
}
