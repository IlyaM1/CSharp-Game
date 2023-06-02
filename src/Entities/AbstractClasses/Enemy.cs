using Dyhar.src.Drawing;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using System.Data;

namespace Dyhar.src.Entities.AbstractClasses;

public abstract class Enemy : MovingGameObject, IWarrior
{
    public Enemy(int x, int y)
    {
        X = x;
        Y = y;
        CurrentHealthPoints = MaxHealthPoints;
        IsAlive = true;
    }

    public override void GotAttackedEventHandler(MeleeWeapon weapon)
    {
        if (LastAttackNumber == weapon.CurrentAttackNumber)
            return;
        else
            LastAttackNumber = weapon.CurrentAttackNumber;

        if (weapon.Attacker is Player)
            CurrentHealthPoints -= weapon.Damage;

        if (CurrentHealthPoints <= 0)
            DyingEventHandler();
    }

    public abstract double GetCurrentHp();
    public virtual void PlayerEnteredScreenEventHandler(Player player) { }

    public abstract void UpdateState(BotStates nextState);

    public bool IsOnPlayerScreen(Camera camera)
    {
        var screenPosition = camera.ConvertMapPositionToScreenPosition(Position);
        if (screenPosition.X >= 0 && screenPosition.X <= Resolution.EtalonWidth)
            if (screenPosition.Y >= 0 && screenPosition.Y <= Resolution.EtalonHeight)
                return true;
        return false;
    }

    public Vector2 GetPosition() => Position;
    public Vector2 GetSize() => new Vector2(Size.Width, Size.Height);
    public double GetMaxHp() => MaxHealthPoints;

    protected double MaxHealthPoints = 100.0;
    protected double CurrentHealthPoints;
    protected ulong LastAttackNumber = 0;
    protected BotStates CurrentState = BotStates.Wander;
}
