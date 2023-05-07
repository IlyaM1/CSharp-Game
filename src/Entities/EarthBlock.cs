using Dyhar.src.Entities.AbstractClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Dyhar.src.Entities;

public class EarthBlock : GameObject
{
    public static Texture2D sprite;

    public EarthBlock(int x, int y)
    {
        X = x;
        Y = y;
        IsSolid = true;
    }

    public override void onUpdate(GameTime gameTime)
    {
        return;
    }

    public override Texture2D GetSprite() => sprite;

    public override void onCollision(GameObject collisionObject)
    {
        return;
    }

    public override void onAttacked(MeleeWeapon weapon)
    {
        Y -= 50;
    }
}
