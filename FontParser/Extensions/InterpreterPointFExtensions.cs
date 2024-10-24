using System.Numerics;
using FontParser.RenderFont.Interpreter;

namespace FontParser.Extensions
{
    public static class InterpreterPointFExtensions
    {
        public static Vector2 ToVector2(this InterpreterPointF interpreterPointF)
        {
            return new Vector2(interpreterPointF.PointF.X, interpreterPointF.PointF.Y);
        }
    }
}