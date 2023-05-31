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
        this._width = width;
        this._height = height;

        IsSolid = true;
    }

    public override Texture2D GetSprite() => _ownSprite != null ? _ownSprite : Sprite;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_ownSprite == null)
            _ownSprite = _getSpriteForThisSize(spriteBatch);

        base.Draw(spriteBatch);
    }

    private int _width;
    private int _height;

    private Texture2D _ownSprite;

    private Texture2D _getSpriteForThisSize(SpriteBatch spriteBatch)
    {
        var stretchedTexture = new Texture2D(spriteBatch.GraphicsDevice, _width, _height);

        var originalColors = new Color[Sprite.Width * Sprite.Height];
        Sprite.GetData(originalColors);
        var color = originalColors[0];

        var stretchedColors = new Color[_width * _height];

        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                stretchedColors[y * _width + x] = color;

        stretchedTexture.SetData(stretchedColors);
        return stretchedTexture;
    }
}
