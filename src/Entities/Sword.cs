using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar.src.Entities
{
    public class Sword : MeleeWeapon
    {
        public static Texture2D sprite;

        public Sword(IWeaponUser attacker) : base(attacker) { }

        public override int WeaponLength { get => sprite.Width + 15; }

        public override Texture2D GetSprite() => sprite;

        private float animationTimer { get => (float)Attacker.GetReload().PassedTime.TotalMilliseconds; }

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

                var angle = MathHelper.Lerp(startAngle, endAngle, animationTimer / AttackDuration);

                var swordPosition = Attacker.FindWeaponStart() + new Vector2(0f, -sprite.Height / 2f);
                swordPosition += new Vector2(0f, -sprite.Height / 2f) * (float)Math.Sin(angle);

                spriteBatch.Draw(sprite, swordPosition, null, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}