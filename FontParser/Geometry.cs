﻿using System.Numerics;

namespace FontParser
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct GlyphPointF
    {
        //from https://docs.microsoft.com/en-us/typography/opentype/spec/glyf
        //'point' of the glyph contour.
        //eg. ... In the glyf table, the position of a point ...
        //  ...  the point is on the curve; otherwise, it is off the curve....

        internal Vector2 P;
        internal bool onCurve;

        public GlyphPointF(float x, float y, bool onCurve)
        {
            P = new Vector2(x, y);
            this.onCurve = onCurve;
        }

        public GlyphPointF(Vector2 position, bool onCurve)
        {
            P = position;
            this.onCurve = onCurve;
        }

        public float X => P.X;
        public float Y => P.Y;

        public static GlyphPointF operator *(GlyphPointF p, float n)
        {
            return new GlyphPointF(p.P * n, p.onCurve);
        }

        //-----------------------------------------

        internal GlyphPointF Offset(short dx, short dy)
        { return new GlyphPointF(new Vector2(P.X + dx, P.Y + dy), onCurve); }

        internal void ApplyScale(float scale)
        {
            P *= scale;
        }

        internal void ApplyScaleOnlyOnXAxis(float scale)
        {
            P = new Vector2(P.X * scale, P.Y);
        }

        internal void UpdateX(float x)
        {
            P.X = x;
        }

        internal void UpdateY(float y)
        {
            P.Y = y;
        }

        internal void OffsetY(float dy)
        {
            P.Y += dy;
        }

        internal void OffsetX(float dx)
        {
            P.X += dx;
        }

#if DEBUG

        internal bool dbugIsEqualsWith(GlyphPointF another)
        {
            return P == another.P && onCurve == another.onCurve;
        }

        public override string ToString()
        { return P.ToString() + " " + onCurve.ToString(); }

#endif
    }
}