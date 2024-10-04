using System;
using System.Numerics;

namespace FontParser.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Rotate(this Vector2 v, double degrees)
        {
            double radians = degrees * (Math.PI / 180);
            return new Vector2(
                (float)(v.X * Math.Cos(radians) - v.Y * Math.Sin(radians)),
                (float)(v.X * Math.Sin(radians) + v.Y * Math.Cos(radians))
            );
        }
    }
}
