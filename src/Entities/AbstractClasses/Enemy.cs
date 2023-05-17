using Dyhar.src.Entities.Interfaces;
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
    public abstract void onPlayerScreen(Player player);

    protected double maxHealthPoints = 100.0;
    protected double currentHealthPoints;
    protected ulong lastAttackNumber = 0;
}
