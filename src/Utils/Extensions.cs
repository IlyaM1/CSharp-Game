using Microsoft.Xna.Framework;
using System;

namespace Dyhar.src.Utils;

public static class RectangleIntersection
{
    public static Rectangle GetIntersection(Rectangle rectA, Rectangle rectB)
    {
        int x = Math.Max(rectA.X, rectB.X);
        int y = Math.Max(rectA.Y, rectB.Y);
        int width = Math.Min(rectA.Right, rectB.Right) - x;
        int height = Math.Min(rectA.Bottom, rectB.Bottom) - y;

        if (width > 0 && height > 0)
            return new Rectangle(x, y, width, height);
        else
            return Rectangle.Empty;
    }
}