using Dyhar.src.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dyhar.src.Drawing;

public sealed class VirtualResolution
{
    public static readonly int etalonWidth = 1600;
    public static readonly int etalonHeight = 900;

    public static readonly int actualWidth = 1600;
    public static readonly int actualHeight = 900;

    public static readonly double kWidth = (double)actualWidth / etalonWidth;
    public static readonly double kHeight = (double)actualHeight / etalonHeight;
    private readonly SpriteBatch _spriteBatch;

    public VirtualResolution(SpriteBatch spriteBatch)
    {
        this._spriteBatch = spriteBatch;
    }

    public void Draw(GameObject gameObject)
    {
        _spriteBatch.Draw(gameObject.GetSprite(),
                    new Rectangle(new Point((int)(gameObject.X * kWidth), (int)(gameObject.Y * kHeight)),
                                            new Point((int)(gameObject.SizeSprite.Width * kWidth), (int)(gameObject.SizeSprite.Height * kHeight))),
                    Color.White);
    }
}
