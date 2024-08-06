using FontParser;
using SkiaSharp;

namespace FontManager.NET.Extensions
{
    public static class GlyphPointFExtensions
    {
        public static SKPoint ToSKPoint(this GlyphPointF point)
        {
            return new SKPoint(point.X, point.Y);
        }
    }
}