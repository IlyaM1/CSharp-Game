using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.LevelsCreator;
using Dyhar.src.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Dyhar.src.Mechanics
{
    public sealed class Physics
    {
        private Level level;
        private float accelerationOfFreeFall { get; set; }

        public Physics(Level level, float accelerationOfFreeFall = 0.6f)
        {
            this.level = level;
            this.accelerationOfFreeFall = accelerationOfFreeFall;
        }

        public void Move(MovingGameObject gameObject)
        {
            var allGameObjects = level.GameObjects;
            NormallyMoveObject(gameObject);

            var collisionedGameObjects = GetCollisionedObjects(gameObject, allGameObjects);
            CallEventOnCollisionForAllCollisionedObjects(collisionedGameObjects, gameObject);

            var solidCollisionedObjects = GetSolidCollisionedObjects(collisionedGameObjects);
            if (solidCollisionedObjects.Count > 0)
            {
                // Next steps if we moved and get collision
                CancelObjectMove(gameObject);
                foreach (var collisionObject in solidCollisionedObjects)
                {
                    var direction = GetDirectionOfCollision(gameObject, collisionObject);
                    MoveObjectClose(gameObject, collisionObject, direction); // Move object as close as it is possible

                    if (direction == Direction.Up || direction == Direction.Down)
                        gameObject.X += gameObject.Force.X; // Normally move horizontally if we are from up or from bottom
                    if (direction == Direction.Left || direction == Direction.Right)
                        gameObject.Y += gameObject.Force.Y; // Normally move horizontally if we are from up or from bottom
                }
            }

            NormalizeObjectPosition(gameObject);
            if (CheckIfObjectIsOnGround(gameObject))
                WhenObjectOnGround(gameObject);
            else
                gameObject.IsOnGround = false;

            gameObject.Force = CountForce(gameObject);
        }

        private Direction GetDirectionOfCollision(MovingGameObject gameObject, GameObject collisionObject)
        {
            var previousState = new Vector2(gameObject.X, gameObject.Y);
            var nextState = new Vector2(gameObject.X + gameObject.Force.X, gameObject.Y + gameObject.Force.Y);
            var intersection = RectangleIntersection.GetIntersection(new Rectangle((int)nextState.X, (int)nextState.Y, gameObject.Width, gameObject.Height), collisionObject.Rectangle);

            if (previousState.X <= collisionObject.X)
                if (previousState.Y <= collisionObject.Y)
                    if (intersection.Width < intersection.Height)
                        return Direction.Left;
                    else
                        return Direction.Up;
                else
                    if (intersection.Width < intersection.Height)
                    return Direction.Left;
                else
                    return Direction.Down;
            else
                if (previousState.Y <= collisionObject.Y)
                if (intersection.Width < intersection.Height)
                    return Direction.Right;
                else
                    return Direction.Up;
            else
                    if (intersection.Width < intersection.Height)
                return Direction.Right;
            else
                return Direction.Down;
        }

        private void MoveObjectClose(MovingGameObject gameObject, GameObject collisionObject, Direction direction)
        {
            if (direction == Direction.Up)
                gameObject.Y += collisionObject.Y - (gameObject.Y + gameObject.Height);
            else if (direction == Direction.Down)
            {
                gameObject.Y -= gameObject.Y - (collisionObject.Y + collisionObject.Height);
                gameObject.Force.Y = 0;
            }
            else if (direction == Direction.Left)
                gameObject.X += collisionObject.X - (gameObject.X + gameObject.Width);
            else if (direction == Direction.Right)
                gameObject.X -= gameObject.X - (collisionObject.X + collisionObject.Width);
            else
            {
                throw new Exception("Unknown direction");
            }

        }

        private void NormallyMoveObject(MovingGameObject gameObject)
        {
            gameObject.X += gameObject.Force.X;
            gameObject.Y += gameObject.Force.Y;
        }

        private void CancelObjectMove(MovingGameObject gameObject)
        {
            gameObject.X -= gameObject.Force.X;
            gameObject.Y -= gameObject.Force.Y;
        }

        private List<GameObject> GetCollisionedObjects(MovingGameObject gameObject, List<GameObject> allGameObjects)
        {
            var list = new List<GameObject>();
            foreach (var otherGameObject in allGameObjects)
                if (gameObject.CheckCollision(otherGameObject))
                    list.Add(otherGameObject);
            return list;
        }

        private Vector2 CountForce(MovingGameObject gameObject)
        {
            var newForce = new Vector2();
            if (gameObject.IsOnGround)
                newForce.Y = 0;
            else
                newForce.Y = (gameObject.Force.Y + accelerationOfFreeFall);

            newForce.X = 0;
            return newForce;
        }

        private void CallEventOnCollisionForAllCollisionedObjects(List<GameObject> gameObjects, MovingGameObject collisionedGameObject)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.onCollision(collisionedGameObject);
                collisionedGameObject.onCollision(gameObject);
            }
        }

        private List<GameObject> GetSolidCollisionedObjects(List<GameObject> collisionedGameObjects)
        {
            var list = new List<GameObject>();
            foreach (var gameObject in collisionedGameObjects)
                if (gameObject.IsSolid)
                    list.Add(gameObject);
            return list;
        }

        private void WhenObjectOnGround(MovingGameObject gameObject)
        {
            gameObject.IsOnGround = true;
            gameObject.onIsOnGround();
        }

        private bool CheckIfObjectIsOnGround(MovingGameObject gameObject)
        {
            if (gameObject.Y + gameObject.Height == level.Height)
                return true;

            foreach (var otherGameObject in level.GameObjects)
                if (otherGameObject.IsSolid)
                    if (gameObject.Y + gameObject.Height == otherGameObject.Y
                        && gameObject.X + gameObject.Width >= otherGameObject.X
                        && gameObject.X <= otherGameObject.X + otherGameObject.Width)
                        return true;

            return false;
        }

        private void NormalizeObjectPosition(MovingGameObject gameObject)
        {
            if (gameObject.Y + gameObject.Height > level.Height)
            {
                gameObject.Y = level.Height - gameObject.Height;
                gameObject.Force.Y = 0;
            }

            if (gameObject.Y < 0)
            {
                gameObject.Force.Y = 0;
                gameObject.Y = 0;
            }

            if (gameObject.X < 0)
                gameObject.X = 0;

            if (gameObject.X + gameObject.Width > level.Width)
                gameObject.X = level.Width - gameObject.Width;
        }
    }
}