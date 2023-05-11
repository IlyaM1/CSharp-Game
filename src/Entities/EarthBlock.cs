using Dyhar.src.Entities.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dyhar.src.Entities;

public class EarthBlock : GameObject
{
    public static Texture2D sprite;
    private int width;
    private int height;

    public override System.Drawing.Size Size => new System.Drawing.Size(width, height);
    private Texture2D ownSprite;

    public EarthBlock(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        this.width = width;
        this.height = height;

        IsSolid = true;
    }

    public override void onUpdate(GameTime gameTime)
    {
        return;
    }

    public override Texture2D GetSprite() => ownSprite != null ? ownSprite : sprite;

    public override void onCollision(GameObject collisionObject)
    {
        return;
    }

    public override void onAttacked(MeleeWeapon weapon)
    {
        Y -= 50;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (ownSprite == null)
            ownSprite = GetSpriteForThisSize(spriteBatch);

        base.Draw(spriteBatch);
    }

    private Texture2D GetSpriteForThisSize(SpriteBatch spriteBatch)
    {
        var stretchedTexture = new Texture2D(spriteBatch.GraphicsDevice, width, height);

        var originalColors = new Color[sprite.Width * sprite.Height];
        sprite.GetData(originalColors);
        var color = originalColors[0];

        var stretchedColors = new Color[width * height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                stretchedColors[y * width + x] = color;

        stretchedTexture.SetData(stretchedColors);
        return stretchedTexture;
    }
}
