using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dyhar.src.Entities;

public class Swordsman : Enemy, IWeaponUser
{
    public override void onPlayerScreen(Player player)
    {
        if (player.X <= X)
        {
            if ((player.Position - Position).Length() >= 120)
                X -= 5;
            direction = Direction.Left;
        }
        else if (player.X > X)
        {
            if ((player.Position - Position).Length() >= 120)
                X += 5;
            direction = Direction.Right;
        }

        Jump();

        // TODO: Make Jump when it's needed

        if (attackAnimationReload.State == ReloadState.NotStarted 
            && delayBetweenAttacks.State == ReloadState.NotStarted)
        {
            onAttack();
        }
    }

    public void Jump()
    {
        if (!IsInJump)
        {
            IsInJump = true;
            Force.Y = -JumpPower;
            return;
        }
    }


    public override void onUpdate(GameTime gameTime)
    {
        CheckAllReloads(gameTime);  
    }

    public static Texture2D sprite;

    public Swordsman(int x, int y) : base(x, y)
    {
        sword = new Sword(this);
        sword.AttackDuration = 500;
        attackAnimationReload = new Reload(sword.AttackDuration);
    }

    public override Texture2D GetSprite() => sprite;
    public override double GetCurrentHp() => currentHealthPoints;

    public Vector2 FindWeaponStart()
    {
        return new Vector2(X + 20, Y + 20);
    }

    public void onAttack()
    {
        sword.onAttack();
        attackAnimationReload.Start();
        delayBetweenAttacks.Start();
    }

    public override void onIsOnGround()
    {
        IsInJump = false;
    }

    private Direction direction = Direction.Left;
    private MeleeWeapon sword;
    private Reload attackAnimationReload;

    public MeleeWeapon GetCurrentWeapon() => sword;
    public Direction GetDirection() => direction;
    public Reload GetAnimationReload() => attackAnimationReload;

    bool IsInJump = false;
    int JumpPower = 18;

    void CheckAllReloads(GameTime gameTime)
    {
        attackAnimationReload.OnUpdate(gameTime);
        delayBetweenAttacks.OnUpdate(gameTime);
    }

    private Reload delayBetweenAttacks = new Reload(1000);
}