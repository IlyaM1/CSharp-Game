using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Dyhar.src.Entities;

public abstract class GameObject
{
    public double X { get; set; }
    public double Y { get; set; }
    public Vector2 Position { get => new Vector2((int)X, (int)Y); }
    public Size SizeSprite { get => new Size(GetSprite().Width, GetSprite().Height); }

    public bool IsSolid { get; set; }


    public abstract void onUpdate(GameTime gameTime);  
    public abstract void onCollision(GameObject collisionObject);
    public abstract Texture2D GetSprite();

    public virtual bool CheckCollision(GameObject otherObject)
    {
        return GameObject.CheckCollision(X, Y, SizeSprite, otherObject.X, otherObject.Y, otherObject.SizeSprite);
    }

    public static bool CheckCollision(double firstObjectX, double firstObjectY, SizeF firstObjectSize,
                                      double secondObjectX, double secondObjectY, SizeF secondObjectSize)
    {
        // смотрим если первый прямоугольник левее второго
        var tooLeft = firstObjectX > secondObjectX + secondObjectSize.Width;
        // смотрим если первый прямоугольник правее второго
        var tooRight = secondObjectX > firstObjectX + firstObjectSize.Width;
        // смотрим если первый прямоугольник выше второго
        var tooHigh = firstObjectY > secondObjectY + secondObjectSize.Height;
        // смотрим если первый прямоугольник ниже второго
        var tooLow = secondObjectY > firstObjectY + firstObjectSize.Height;

        // если ни одно из условий непересечения не сработало, значит они перескаются
        return !(tooLeft || tooRight || tooHigh || tooLow);
    }

    public virtual void onAttacked(MeleeWeapon weapon)
    {
        return;
    }
}