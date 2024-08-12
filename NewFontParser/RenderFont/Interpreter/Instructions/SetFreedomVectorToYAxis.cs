using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    internal class SetFreedomVectorToYAxis : IInstruction
    {
        public byte OpCode => 0x04;

        public string Mnemonic => "SFVTCA[Y]";

        public string Description => "Set freedom vector to Y-axis";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            graphicsState.FreedomVector = new PointF(0, 1);
        }
    }
}
