using System.Drawing;
using SkiaSharp;

namespace FontManager.NET.Extensions
{
    public static class PointExtensions
    {
        public static SKPoint ToSkPoint(this Point p)
        {
            return new SKPoint(p.X, p.Y);
        }
    }
}
