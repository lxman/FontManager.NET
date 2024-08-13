using System.Drawing;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class SimpleGlyphCoordinate
    {
        public Point Point { get; }

        public bool OnCurve { get; }

        public SimpleGlyphCoordinate(Point point, bool onCurve)
        {
            Point = point;
            OnCurve = onCurve;
        }

        public override string ToString()
        {
            return $"Point: {Point}, OnCurve: {OnCurve}";
        }
    }
}