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
    public bool IsAlive { get => _isAlive; protected set { _isAlive = value; } }

    public bool IsSolid { get; protected set; }

    public virtual void UpdatingEventHandler(GameTime gameTime) { }
    public virtual void CollisionedEventHandler(GameObject collisionObject) { }

    public abstract Texture2D GetSprite();

    public virtual bool CheckCollision(GameObject otherObject)
    {
        return _сheckCollision(X, Y, Size, otherObject.X, otherObject.Y, otherObject.Size);
    }

    public virtual void GotAttackedEventHandler(MeleeWeapon weapon) { }

    public virtual void DyingEventHandler()
    {
        IsAlive = false;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(GetSprite(), Position, Color.White);
    }

    private bool _isAlive = true;

    private bool _сheckCollision(double firstObjectX, double firstObjectY, SizeF firstObjectSize,
        double secondObjectX, double secondObjectY, SizeF secondObjectSize)
    {
        var isCollisionFromLeft = firstObjectX + firstObjectSize.Width > secondObjectX;
        var isCollisionFromRight = firstObjectX < secondObjectX + secondObjectSize.Width;
        var isCollisionFromUp = firstObjectY + firstObjectSize.Height > secondObjectY;
        var isCollisionFromBottom = firstObjectY < secondObjectY + secondObjectSize.Height;

        return isCollisionFromLeft && isCollisionFromRight && isCollisionFromUp && isCollisionFromBottom;
    }
}