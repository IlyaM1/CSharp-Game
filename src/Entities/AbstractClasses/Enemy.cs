using Dyhar.src.Drawing;
using Dyhar.src.Entities.Interfaces;
using Microsoft.Xna.Framework;

namespace Dyhar.src.Entities.AbstractClasses;

public abstract class Enemy : MovingGameObject, IWarrior
{
    public Enemy(int x, int y)
    {
        X = x;
        Y = y;
        currentHealthPoints = maxHealthPoints;
        IsAlive = true;
    }

    public override void onAttacked(MeleeWeapon weapon)
    {
        if (lastAttackNumber == weapon.CurrentAttackNumber)
            return;
        else
            lastAttackNumber = weapon.CurrentAttackNumber;

        currentHealthPoints -= weapon.Damage;
        if (currentHealthPoints <= 0)
            onDeath();
    }

    public abstract double GetCurrentHp();
    public virtual void onPlayerScreen(Player player)
    {
        return;
    }

    public bool IsOnPlayerScreen(Camera camera)
    {
        var screenPosition = camera.MapPositionToScreenPosition(Position);
        if (screenPosition.X >= 0 && screenPosition.X <= Resolution.etalonWidth)
            if (screenPosition.Y >= 0 && screenPosition.Y <= Resolution.etalonHeight)
                return true;
        return false;
    }

    public Vector2 GetPosition() => Position;
    public Vector2 GetSize() => new Vector2(Size.Width, Size.Height);
    public double GetMaxHp() => maxHealthPoints;

    protected double maxHealthPoints = 100.0;
    protected double currentHealthPoints;
    protected ulong lastAttackNumber = 0;
}
