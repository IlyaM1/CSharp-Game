namespace Dyhar.src.Drawing;

public struct Resolution
{
    public static readonly int etalonWidth = 1600;
    public static readonly int etalonHeight = 900;

    public static readonly int actualWidth = 1600;
    public static readonly int actualHeight = 900;

    public static readonly double kWidth = (double)actualWidth / etalonWidth;
    public static readonly double kHeight = (double)actualHeight / etalonHeight;
}