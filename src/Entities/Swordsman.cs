using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Dyhar.src.Entities;

internal class Path
{
    public Queue path = new Queue();
}

public class Swordsman : Enemy, IWeaponUser
{
    
    Path FindPathToPlayer(Player player)
    {
        return new Path();
    }

    void MoveWithPath()
    {
        if (currentPath is null)
            return;
    }

    public override void onPlayerScreen(Player player)
    {
        var path = FindPathToPlayer(player);
        return;
    }

    public override void onCollision(GameObject collisionObject)
    {
        return;
    }

    public override void onIsOnGround()
    {
        return;
    }

    

    public override void onUpdate(GameTime gameTime)
    {
        CheckAllReloads(gameTime);

        if (attackAnimationReload.State == ReloadState.NotStarted)
            onAttack();
    }

    public static Texture2D sprite;

    private Path currentPath;

    public Swordsman(int x, int y) : base(x, y)
    {
        sword = new Sword(this);
        sword.AttackDuration = 1500;
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
    }

    private Direction direction = Direction.Left;
    private MeleeWeapon sword;
    private Reload attackAnimationReload;

    public MeleeWeapon GetCurrentWeapon() => sword;
    public Direction GetDirection() => direction;
    public Reload GetAnimationReload() => attackAnimationReload;

    void CheckAllReloads(GameTime gameTime)
    {
        attackAnimationReload.OnUpdate(gameTime);
    }
}