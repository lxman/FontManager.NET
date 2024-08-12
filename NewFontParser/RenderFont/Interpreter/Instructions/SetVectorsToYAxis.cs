using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetVectorsToYAxis : IInstruction
    {
        public byte OpCode => 0x00;
        public string Mnemonic => "SVTCA[Y]";
        public string Description => "Set freedom and projection vectors to the y-axis";

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
            graphicsState.ProjectionVector = new PointF(0, 1);
        }
    }
}
