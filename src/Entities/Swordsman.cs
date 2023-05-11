using Dyhar.src.Entities.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Dyhar.src.Entities;

internal class Path
{
    public Queue path = new Queue();
}

public class Swordsman : Enemy
{
    public static Texture2D sprite;

    private double maxHealthPoints = 100.0;
    private double currentHealthPoints = 100.0;

    private Path currentPath;

    public Swordsman(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override void onCollision(GameObject collisionObject)
    {
        return;
    }

    public override void onIsOnGround()
    {
        return;
    }

    public override void onPlayerScreen(Player player)
    {
        var path = FindPathToPlayer(player);
        return;
    }

    public override void onUpdate(GameTime gameTime)
    {
        MoveWithPath();
    }

    public override Texture2D GetSprite() => sprite;
    public override double GetCurrentHp() => currentHealthPoints;

    Path FindPathToPlayer(Player player)
    {
        return new Path();
    }

    void MoveWithPath()
    {
        if (currentPath is null)
            return;
    }
}