using Microsoft.Xna.Framework;

namespace Dyhar.src.Entities
{
    public abstract class MovingGameObject : GameObject
    {
        public Vector2 Force;
        public double Speed { get; set; }
        public bool IsOnGround { get; set; }

        public abstract void onIsOnGround();
    }
}