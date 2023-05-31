namespace Dyhar.src.Drawing;

public readonly struct Resolution
{
    public static readonly int EtalonWidth = 1600;
    public static readonly int EtalonHeight = 900;

    public static readonly int ActualWidth = 1600;
    public static readonly int ActualHeight = 900;

    public static readonly double kWidth = (double)ActualWidth / EtalonWidth;
    public static readonly double kHeight = (double)ActualHeight / EtalonHeight;
}