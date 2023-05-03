using Microsoft.Xna.Framework.Graphics;

namespace Dyhar.src.Entities
{
    public class Sword : MeleeWeapon
    {
        public static Texture2D sprite;

        public Sword(IWeaponUser attacker) : base(attacker) { }

        public override int WeaponLength { get => 30; }

        public override Texture2D GetSprite() => sprite;
    }
}
