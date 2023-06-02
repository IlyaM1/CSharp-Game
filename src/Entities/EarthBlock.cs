using Dyhar.src.Entities.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dyhar.src.Entities;

public class EarthBlock : GameObject
{
    public static Texture2D Sprite;
    public override System.Drawing.Size Size => new System.Drawing.Size(_width, _height);

    public EarthBlock(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        _width = width;
        _height = height;

        IsSolid = true;
    }

    public override Texture2D GetSprite() => Sprite;

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, Rectangle, Color.White);
    }

    private int _width;
    private int _height;
}
