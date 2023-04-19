using Dyhar.src.Entities;
using Dyhar.src.LevelsCreator;
using System;
using System.Numerics;

namespace Dyhar.src.Mechanics
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

        public bool IsOnGround(MovingGameObject gameObject, Level level) 
        {
            if (gameObject == null)
                throw new NullReferenceException();

            if (Dyhar.height - gameObject.Y <= gameObject.SizeSprite.Height)
            {
                gameObject.OnIsOnGround();
                return true;
            }
                
            return false;
        }

        public Vector2 CountForce(MovingGameObject gameObject, Level level)
        {
            if (gameObject == null)
                throw new NullReferenceException();

            var startForce = gameObject.Force;
            var newForce = new Vector2();

            if (IsOnGround(gameObject, level))
                newForce.Y = 0;
            else
                newForce.Y = (float)(startForce.Y + accelerationOfFreeFall);

            newForce.X = startForce.X;
            //if (startForce.X < 0)
            //    newForce.X = (float)(startForce.X - (frictionСoefficient * startForce.Y));
            //else if (startForce.X > 0)
            //    newForce.X = (float)(startForce.X - (frictionСoefficient * startForce.Y));

            return newForce;
        }

        public void Move(MovingGameObject gameObject, Level level)
        {
            gameObject.X += gameObject.Force.X;
            gameObject.Force.X = 0;

            gameObject.Y += gameObject.Force.Y;
            if (Dyhar.height - gameObject.Y <= gameObject.SizeSprite.Height)
                gameObject.Y = Dyhar.height - gameObject.SizeSprite.Height;

            gameObject.Force = CountForce(gameObject, level);
        }
    }
}