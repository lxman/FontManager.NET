using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetVectorsToXAxis : IInstruction
    {
        public byte OpCode => 0x01;
        public string Mnemonic => "SVTCA[X]";
        public string Description => "Set freedom and projection vectors to the x-axis";

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
            graphicsState.ProjectionVector = new PointF(1, 0);
        }
    }
}
