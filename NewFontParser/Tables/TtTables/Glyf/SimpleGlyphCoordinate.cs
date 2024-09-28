﻿using System.Drawing;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public struct SimpleGlyphCoordinate
    {
        public PointF Point { get; private set; }

        public bool OnCurve { get; private set; }

        public SimpleGlyphCoordinate(PointF point, bool onCurve)
        {
            Point = point;
            OnCurve = onCurve;
        }

        public void ChangePoint(PointF newPoint, bool onCurve)
        {
            Point = newPoint;
            OnCurve = onCurve;
        }

        public override string ToString()
        {
            return $"Point: {Point.X}, {Point.Y} OnCurve: {OnCurve}";
        }
    }
}