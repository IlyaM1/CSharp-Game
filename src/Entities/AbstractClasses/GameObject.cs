using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Dyhar.src.Entities.AbstractClasses;

public abstract class GameObject
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vector2 Position => new Vector2((int)X, (int)Y);
    public virtual Size Size => new Size(GetSprite().Width, GetSprite().Height);
    public Rectangle Rectangle => new Rectangle((int)X, (int)Y, Size.Width, Size.Height);
    public int Width => Size.Width;
    public int Height => Size.Height;
    public bool IsAlive { get => isAlive; protected set { isAlive = value; } }

    public bool IsSolid { get; protected set; }

    public virtual void onUpdate(GameTime gameTime) { }
    public virtual void onCollision(GameObject collisionObject) { }

    public abstract Texture2D GetSprite();

    public virtual bool CheckCollision(GameObject otherObject)
    {
        return CheckCollision(X, Y, Size, otherObject.X, otherObject.Y, otherObject.Size);
    }

    public virtual void onAttacked(MeleeWeapon weapon) { }

    public virtual void onDeath()
    {
        IsAlive = false;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(GetSprite(), Position, Color.White);
    }

    private bool isAlive = true;

    private bool CheckCollision(double firstObjectX, double firstObjectY, SizeF firstObjectSize,
        double secondObjectX, double secondObjectY, SizeF secondObjectSize)
    {
        var isCollisionFromLeft = firstObjectX + firstObjectSize.Width > secondObjectX;
        var isCollisionFromRight = firstObjectX < secondObjectX + secondObjectSize.Width;
        var isCollisionFromUp = firstObjectY + firstObjectSize.Height > secondObjectY;
        var isCollisionFromBottom = firstObjectY < secondObjectY + secondObjectSize.Height;

        return isCollisionFromLeft && isCollisionFromRight && isCollisionFromUp && isCollisionFromBottom;
    }
}