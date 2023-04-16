using Dyhar.src.Entities;
using Dyhar.src.LevelsCreator;
using System;
using System.Numerics;

namespace Dyhar.src.Physics
{
    public class Physic
    {
        private double frictionСoefficient { get; set; }
        private double accelerationOfFreeFall { get; set; }

        public Physic(double frictionСoefficient = 1, double accelerationOfFreeFall = 1)
        {
            this.frictionСoefficient = frictionСoefficient;
            this.accelerationOfFreeFall = accelerationOfFreeFall;
        }

        public bool IsOnGround(GameObject gameObject, Level level) 
        {
            if (gameObject == null)
                throw new NullReferenceException();

            //if (gameObject.Y >= Dyhar.height - gameObject.Size.Height)
            //    return true;

            return false;
        }

        public Vector2 CountForce(GameObject gameObject, Level level)
        {
            if (gameObject == null)
                throw new NullReferenceException();

            var startForce = gameObject.Force;
            var newForce = new Vector2();

            if (IsOnGround(gameObject, level))
                newForce.Y = 0;
            else
                newForce.Y = (float)(startForce.Y + accelerationOfFreeFall);

            if (startForce.X < 0)
                newForce.X = (float)(startForce.X + (frictionСoefficient * startForce.Y));
            else if (startForce.X > 0)
                newForce.X = (float)(startForce.X - (frictionСoefficient * startForce.Y));

            return newForce;
        }

        public void Move(GameObject gameObject, Level level)
        {
            gameObject.X += gameObject.Force.X;
            gameObject.Y += gameObject.Force.Y;
            if (gameObject.Y >= Dyhar.height - gameObject.Size.Height)
                gameObject.Y = Dyhar.height - gameObject.Size.Height - 100;
            gameObject.Force = CountForce(gameObject, level);
        }
    }
}