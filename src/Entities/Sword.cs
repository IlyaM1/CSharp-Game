using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar.src.Entities;

public class Sword : MeleeWeapon
{
    public static Texture2D Sprite;

    public Sword(IWeaponUser attacker) : base(attacker)
    {
        Damage = 35;
    }

    public override int WeaponLength => Sprite.Width + 5;
    public override Texture2D GetSprite() => Sprite;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Attacker.IsAttacking())
        {
            var startAngle = 0f;
            var endAngle = 0f;

            if (Attacker.GetDirection() == Direction.Right)
            {
                startAngle = MathHelper.PiOver2;
                endAngle = -MathHelper.PiOver2;
            }
            else if (Attacker.GetDirection() == Direction.Left)
            {
                startAngle = MathHelper.PiOver2;
                endAngle = MathHelper.Pi + MathHelper.PiOver2;
            }

            var angle = MathHelper.Lerp(startAngle, endAngle, _animationTimer / AttackDuration);

            var swordPosition = Attacker.GetWeaponStartPosition() + new Vector2(0f, -Sprite.Height / 2f);
            swordPosition += new Vector2(0f, -Sprite.Height / 2f) * (float)Math.Sin(angle);

            spriteBatch.Draw(Sprite, swordPosition, null, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }

    private float _animationTimer => (float)Attacker.GetAnimationReload().PassedTimeInMilliseconds.TotalMilliseconds;
}