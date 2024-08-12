using System.Collections.Generic;
using System.Drawing;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetProjectionVectorToYAxis : IInstruction
    {
        public byte OpCode => 0x02;
        public string Mnemonic => "SPVTCA[0]";
        public string Description => "Set Projection Vector to Y Axis";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            graphicsState.ProjectionVector = new PointF(0, 1);
        }
    }
}
