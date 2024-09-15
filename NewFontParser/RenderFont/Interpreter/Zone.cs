using System.Drawing;
using System.Linq;
using NewFontParser.Tables.TtTables.Glyf;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NewFontParser.RenderFont.Interpreter
{
    public class Zone
    {
        public bool IsTwilight { get; private set; }

        public InterpreterPointF[] Current { get; private set; }

        public InterpreterPointF[] Original { get; private set; }

        public Zone()
        {
        }

        public Zone(bool isTwilight, SimpleGlyphCoordinate[] original)
        {
            IsTwilight = isTwilight;
            Current = original.Select(c => new InterpreterPointF(c)).ToArray();
        }

        public Zone(bool isTwilight, PointF[] original)
        {
            IsTwilight = isTwilight;
            Original = original.Select(o => new InterpreterPointF(o)).ToArray();
            Current = original.Select(o => new InterpreterPointF(o)).ToArray();
        }

        public Zone(bool isTwilight, InterpreterPointF[] original)
        {
            IsTwilight = isTwilight;
            Original = original;
            Current = original;
        }

        public void Initialize(bool isTwilight, PointF[] original)
        {
            IsTwilight = isTwilight;
            Original = original.Select(o => new InterpreterPointF(o)).ToArray();
            Current = original.Select(o => new InterpreterPointF(o)).ToArray();
        }

        public void Initialize(bool isTwilight, InterpreterPointF[] original)
        {
            IsTwilight = isTwilight;
            Original = original;
            Current = original;
        }

        public void MovePoint(int index, PointF newPoint)
        {
            Current[index].MovePoint(newPoint);
        }
    }
}
