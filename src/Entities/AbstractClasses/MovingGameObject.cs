using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;
using System;

namespace Dyhar.src.Entities.AbstractClasses
{
    public abstract class MovingGameObject : GameObject
    {
        public Vector2 Force;
        public float Speed { get; set; }
        public bool IsOnGround { get; set; }

        public virtual void MoveHorizontally(Direction direction)
        {
            if (direction == Direction.Left)
                Force.X -= Speed;
            else if (direction == Direction.Right)
                Force.X += Speed;
            else
                throw new ArgumentException("Impossible Direction");
        }

        public virtual void FalledOnGroundEventHandler() { }
    }
}