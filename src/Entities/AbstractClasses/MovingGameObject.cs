using Microsoft.Xna.Framework;

namespace Dyhar.src.Entities.AbstractClasses
{
    public abstract class MovingGameObject : GameObject
    {
        public Vector2 Force;
        public float Speed { get; set; }
        public bool IsOnGround { get; set; }

        public virtual void onIsOnGround() { }
    }
}