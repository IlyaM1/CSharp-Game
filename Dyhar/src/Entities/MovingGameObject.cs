using Microsoft.Xna.Framework;
using System.Drawing;

namespace Dyhar.src.Entities
{
    public abstract class MovingGameObject : GameObject
    {
        public Vector2 Force;
        public double Speed { get; set; }

        public abstract void OnIsOnGround();
    }
}
