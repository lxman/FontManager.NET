using System;
using System.Drawing;
using FontParser.Tables.TtTables.Glyf;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace FontParser.RenderFont.Interpreter
{
    public class InterpreterPointF
    {
        public PointF PointF { get; private set; }

        public bool OnCurve { get; }

        public TouchState TouchState { get; private set; } = TouchState.None;

        public InterpreterPointF()
        {
        }

        public InterpreterPointF(SimpleGlyphCoordinate coordinate)
        {
            PointF = coordinate.Point;
            OnCurve = coordinate.OnCurve;
        }

        public InterpreterPointF(PointF pointF)
        {
            PointF = pointF;
        }

        public InterpreterPointF(PointF pointF, TouchState touchState)
        {
            PointF = pointF;
            TouchState |= touchState;
        }

        public void MovePoint(PointF newPoint)
        {
            if (Math.Abs(newPoint.X - PointF.X) > float.MinValue)
            {
                TouchState |= TouchState.X;
            }
            if (Math.Abs(newPoint.Y - PointF.Y) > float.MinValue)
            {
                TouchState |= TouchState.Y;
            }
            PointF = newPoint;
        }

        public void SetTouchState(TouchState touchState)
        {
            TouchState = touchState;
        }
    }
}
