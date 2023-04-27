using Dyhar.src.Drawing;
using Dyhar.src.Entities;
using Dyhar.src.LevelsCreator;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Dyhar.src.Mechanics
{
    public class Physic
    {
        private Level level;
        private double frictionСoefficient { get; set; }
        private double accelerationOfFreeFall { get; set; }

        public Physic(Level level, double frictionСoefficient = 1, double accelerationOfFreeFall = 0.6)
        {
            this.frictionСoefficient = frictionСoefficient;
            this.accelerationOfFreeFall = accelerationOfFreeFall;
            this.level = level;
        }

        public void CheckObjectIsOnGround(MovingGameObject gameObject) 
        {
            if (VirtualResolution.etalonHeight - gameObject.Y == gameObject.SizeSprite.Height)
            {
                gameObject.onIsOnGround();
                gameObject.IsOnGround = true;
                return;
            }

            foreach (var otherGameObject in GetSolidCollisionedGameObjects(gameObject))
            {
                var bottomGameObject = gameObject.Y + gameObject.SizeSprite.Height;
                if (bottomGameObject == otherGameObject.Y)
                {
                    gameObject.onIsOnGround();
                    gameObject.IsOnGround = true;
                    return;
                }
            }

            gameObject.IsOnGround = false;
        }

        public Vector2 CountForce(MovingGameObject gameObject)
        {
            var newForce = new Vector2();

            if (gameObject.IsOnGround)
                newForce.Y = 0;
            else
                newForce.Y = (float)(gameObject.Force.Y + accelerationOfFreeFall);

            newForce.X = 0;

            return newForce;
        }

        public void Move(MovingGameObject gameObject)
        {
            if (gameObject is null)
                throw new ArgumentNullException();

            // Move object and check if we get collision
            gameObject.X += gameObject.Force.X;
            gameObject.Y += gameObject.Force.Y;

            var collisionedGameObjectList = GetCollisionedGameObjects(gameObject);
            CallOnCollisionEventForAllCollisionedObjects(gameObject, collisionedGameObjectList);

            if (!CanObjectBeMoved(collisionedGameObjectList))
            {
                if (!gameObject.IsOnGround)
                {
                    gameObject.X -= gameObject.Force.X;
                    gameObject.Y -= gameObject.Force.Y;
                    foreach (var collisionedGameObject in GetSolidCollisionedGameObjects(collisionedGameObjectList))
                    {
                        var directionOfCollision = FindDirectionOfCollision(gameObject, collisionedGameObject);
                        MoveObjectCloseAsPossible(gameObject, collisionedGameObject, directionOfCollision);
                        if (directionOfCollision == Direction.Left || directionOfCollision == Direction.Right)
                            gameObject.Y += gameObject.Force.Y;
                        else if (directionOfCollision == Direction.Up || directionOfCollision == Direction.Down)
                        {
                            gameObject.X += gameObject.Force.X;
                            gameObject.Force.Y = 0;
                        }
                            
                    }
                }
                else
                {
                    gameObject.Y -= gameObject.Force.Y;
                }
            }

                if (VirtualResolution.etalonHeight - gameObject.Y <= gameObject.SizeSprite.Height)
                gameObject.Y = VirtualResolution.etalonHeight - gameObject.SizeSprite.Height;

            CheckObjectIsOnGround(gameObject);
            gameObject.Force = CountForce(gameObject);
        }

        public Direction FindDirectionOfCollision(MovingGameObject gameObject, GameObject collisionedGameObject)
        {
            var bottomGameObject = gameObject.Y + gameObject.SizeSprite.Height;
            if (bottomGameObject <= collisionedGameObject.Y)
                return Direction.Up;

            var bottomOtherGameObject = collisionedGameObject.Y + collisionedGameObject.SizeSprite.Height;
            if (gameObject.Y >= bottomOtherGameObject)
                return Direction.Down;

            var rightPartGameObject = gameObject.X + gameObject.SizeSprite.Width;
            if (collisionedGameObject.X >= rightPartGameObject)
                return Direction.Left;

            var rightPartOtherGameObject = collisionedGameObject.X + collisionedGameObject.SizeSprite.Width;
            if (gameObject.X >= rightPartOtherGameObject)
                return Direction.Right;

            throw new ArgumentException();
        }

        public void MoveObjectCloseAsPossible(MovingGameObject gameObject, GameObject collisionedGameObject, Direction directionOfCollision)
        {
            if (directionOfCollision == Direction.Left)
                gameObject.X += (collisionedGameObject.X - (gameObject.X + gameObject.SizeSprite.Width));

            else if (directionOfCollision == Direction.Right)
                gameObject.X -= (gameObject.X - (collisionedGameObject.X + collisionedGameObject.SizeSprite.Width));

            else if (directionOfCollision == Direction.Up)
                gameObject.Y += (collisionedGameObject.Y - (gameObject.Y + gameObject.SizeSprite.Height));

            else if (directionOfCollision == Direction.Down)
                gameObject.Y -= (gameObject.Y - (collisionedGameObject.Y + collisionedGameObject.SizeSprite.Height));
        }

        public bool CanObjectBeMoved(List<GameObject> gameObjectList)
        {
            foreach (var gameObject in gameObjectList)
                if (gameObject.IsSolid)
                    return false;
            return true;
        }

        public List<GameObject> GetCollisionedGameObjects(GameObject gameObject)
        {
            var collisionedGameObjects = new List<GameObject>();

            foreach (var otherGameObject in level.gameObjects)
                if (gameObject.CheckCollision(otherGameObject))
                    collisionedGameObjects.Add(otherGameObject);

            return collisionedGameObjects;
        }

        public List<GameObject> GetSolidCollisionedGameObjects(List<GameObject> gameObjectList)
        {
            var solidObjectsList = new List<GameObject>();
            foreach(var gameObject in gameObjectList)
                if (gameObject.IsSolid)
                    solidObjectsList.Add(gameObject);
            return solidObjectsList;
        }

        public List<GameObject> GetSolidCollisionedGameObjects(GameObject gameObject)
        {
            var solidObjectsList = new List<GameObject>();
            foreach (var otherGameObject in GetCollisionedGameObjects(gameObject))
                if (otherGameObject.IsSolid)
                    solidObjectsList.Add(otherGameObject);
            return solidObjectsList;
        }

        public void CallOnCollisionEventForAllCollisionedObjects(GameObject checkablegameObject, List<GameObject> gameObjectList)
        {
            foreach (var gameObject in gameObjectList)
                if (gameObject.IsSolid)
                    gameObject.onCollision(checkablegameObject);
        }
    }
}