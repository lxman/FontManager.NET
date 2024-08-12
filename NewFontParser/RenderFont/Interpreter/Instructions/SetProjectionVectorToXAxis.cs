using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetProjectionVectorToXAxis : IInstruction
    {
        public byte OpCode => 0x03;
        public string Mnemonic => "SPVTCA[1]";
        public string Description => "Set Projection Vector to X Axis";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            graphicsState.ProjectionVector = new PointF(1, 0);
        }
    }
}
