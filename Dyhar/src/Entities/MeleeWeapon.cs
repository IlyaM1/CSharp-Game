using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar.src.Entities;

public abstract class MeleeWeapon
{
    public IWeaponUser Attacker { get; private set; }
    public abstract int WeaponLength { get; }
    public readonly int AttackDuration = 250; // Adjust this value to control the animation speed

    public MeleeWeapon(IWeaponUser attacker)
    {
        Attacker = attacker;
    }

    public bool CheckCollision(GameObject gameObject, Direction direction)
    {
        var attackPoint = Attacker.FindWeaponStart();
        if (direction == Direction.Right && attackPoint.X <= gameObject.X)
            return Math.Pow(gameObject.X - attackPoint.X, 2) + Math.Pow(gameObject.Y - attackPoint.Y, 2) <= (WeaponLength * WeaponLength);
        else if (direction == Direction.Left && attackPoint.X >= gameObject.X)
            return Math.Pow(attackPoint.X - gameObject.X, 2) + Math.Pow(attackPoint.Y - gameObject.Y, 2) <= (WeaponLength * WeaponLength);

        return false;
    }

    public abstract Texture2D GetSprite();
    public abstract void Draw(SpriteBatch spriteBatch);
}