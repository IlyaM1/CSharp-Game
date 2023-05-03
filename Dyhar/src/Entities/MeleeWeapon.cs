using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework.Graphics;

namespace Dyhar.src.Entities;

public abstract class MeleeWeapon
{
    public IWeaponUser Attacker { get; private set; }
    public abstract int WeaponLength { get; }
    

    public MeleeWeapon(IWeaponUser attacker)
    {
        Attacker = attacker;
    }

    public bool CheckCollision(GameObject gameObject, Direction direction)
    {
        var attackPoint = Attacker.FindWeaponStart();
        if (direction == Direction.Right && attackPoint.X <= gameObject.X)
            return (gameObject.X - attackPoint.X) + (gameObject.Y - attackPoint.Y) <= (WeaponLength * WeaponLength);
        else if (direction == Direction.Left && attackPoint.X >= gameObject.X)
            return (attackPoint.X - gameObject.X) + (attackPoint.Y - gameObject.Y) <= (WeaponLength * WeaponLength);

        return false;
    }

    public abstract Texture2D GetSprite();
}