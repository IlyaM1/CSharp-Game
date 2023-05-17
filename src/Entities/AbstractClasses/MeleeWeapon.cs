using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar.src.Entities.AbstractClasses;

public abstract class MeleeWeapon
{
    public IWeaponUser Attacker { get; private set; }
    public abstract int WeaponLength { get; }
    public int AttackDuration { get; set; }
    public int Damage { get; set; }
    public ulong CurrentAttackNumber { get; protected set; }

    public MeleeWeapon(IWeaponUser attacker)
    {
        Attacker = attacker;
        AttackDuration = 250;
        Damage = 10;
        CurrentAttackNumber = 0;
    }

    public virtual void onAttack()
    {
        CurrentAttackNumber++;
    }

    public bool CheckCollision(GameObject gameObject, Direction direction)
    {
        if (gameObject == Attacker)
            return false;

        var attackPoint = Attacker.FindWeaponStart();

        if ((direction == Direction.Right && attackPoint.X <= gameObject.X) 
            || (direction == Direction.Left && attackPoint.X >= gameObject.X))
            return IntersectsCircleRectangle(attackPoint, WeaponLength, gameObject.Rectangle);

        return false;
    }

    public abstract Texture2D GetSprite();
    public abstract void Draw(SpriteBatch spriteBatch);

    private bool IntersectsCircleRectangle(Vector2 circleCenter, float radius, Rectangle rectangle)
    {
        var clampedX = Math.Max(rectangle.Left, Math.Min(circleCenter.X, rectangle.Right));
        var clampedY = Math.Max(rectangle.Top, Math.Min(circleCenter.Y, rectangle.Bottom));
        var distanceX = circleCenter.X - clampedX;
        var distanceY = circleCenter.Y - clampedY;
        var distanceSq = distanceX * distanceX + distanceY * distanceY;
        var radiusSq = radius * radius;

        return (distanceSq <= radiusSq) || (
            (circleCenter.X >= rectangle.Left && circleCenter.X <= rectangle.Right) &&
            (circleCenter.Y >= rectangle.Top && circleCenter.Y <= rectangle.Bottom)
        );
    }
}