using System.Collections.Generic;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

namespace NewFontParser.RenderFont.Interpreter.Instructions
{
    public class SetProjectionVectorToLine : IInstruction
    {
        public byte OpCode => 0x06;

        public string Mnemonic => "SPVTL[0]";

        public string Description => "Set the projection vector to the line connecting the origin and the point (x, y).";

        public void Execute
        (byte opCode,
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            Table glyphTable,
            GraphicsState graphicsState,
            Stack<int> stack)
        {
            //if (stack.Count < 2)
            //{
            //    throw new InvalidOperationException("The stack must contain at least two values.");
            //}

            //int y = stack.Pop();
            //int x = stack.Pop();
            //PointF startPoint = graphicsState.GlyphPoints[x];

            //graphicsState.ProjectionVector = new System.Drawing.PointF(x, y);
        }
    }
}
