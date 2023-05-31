using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Levels;
using Dyhar.src.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyhar.src.Mechanics;

public class Physics
{
    private Level _level;
    private float _accelerationOfFreeFall { get; set; }

    public Physics(Level level, float accelerationOfFreeFall = 0.6f)
    {
        _level = level;
        _accelerationOfFreeFall = accelerationOfFreeFall;
    }

    public void HandleObjectMovementAndCollisions(MovingGameObject gameObject)
    {
        var allGameObjects = _level.GameObjects;
        _normallyMoveObject(gameObject);

        var collisionedGameObjects = _getCollisionedObjects(gameObject, allGameObjects);

        var solidCollisionedObjects = _getSolidCollisionedObjects(collisionedGameObjects);
        if (solidCollisionedObjects.Count > 0)
        {
            // Next steps if we moved and get collision
            _cancelObjectMove(gameObject);
            var collisionObject = solidCollisionedObjects.FirstOrDefault();
            var direction = _getDirectionOfCollision(gameObject, collisionObject);
            _moveObjectClose(gameObject, collisionObject, direction); // Move object as close as it is possible

            if (direction == Direction.Up || direction == Direction.Down)
                gameObject.X += gameObject.Force.X; // Normally move horizontally if we are from up or from bottom
            if (direction == Direction.Left || direction == Direction.Right)
                gameObject.Y += gameObject.Force.Y; // Normally move horizontally if we are from up or from bottom
        }

        _normalizeObjectPosition(gameObject);
        if (_checkIfObjectIsOnGround(gameObject))
            _whenObjectOnGround(gameObject);
        else
            gameObject.IsOnGround = false;

        gameObject.Force = _countForce(gameObject);
        _callEventOnCollisionForAllCollisionedObjects(collisionedGameObjects, gameObject);
    }

    private Direction _getDirectionOfCollision(MovingGameObject gameObject, GameObject collisionObject)
    {
        var previousState = new Vector2(gameObject.X, gameObject.Y);
        var nextState = new Vector2(gameObject.X + gameObject.Force.X, gameObject.Y + gameObject.Force.Y);
        var intersection = RectangleIntersection.GetIntersection(new Rectangle((int)nextState.X, (int)nextState.Y, gameObject.Width, gameObject.Height), collisionObject.Rectangle);

        if (previousState.X <= collisionObject.X)
        {
            if (previousState.Y <= collisionObject.Y)
            {
                if (intersection.Width < intersection.Height)
                    return Direction.Left;
                else
                    return Direction.Up;
            }
            else
            {
                if (intersection.Width < intersection.Height)
                    return Direction.Left;
                else
                    return Direction.Down;
            }        
        }
        else
        {
            if (previousState.Y <= collisionObject.Y)
            {
                if (intersection.Width < intersection.Height)
                    return Direction.Right;
                else
                    return Direction.Up;
            }
            else
            {
                if (intersection.Width < intersection.Height)
                    return Direction.Right;
                else
                    return Direction.Down;
            }      
        }    
    }


    private void _moveObjectClose(MovingGameObject gameObject, GameObject collisionObject, Direction direction)
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

    private void _normallyMoveObject(MovingGameObject gameObject)
    {
        gameObject.X += gameObject.Force.X;
        gameObject.Y += gameObject.Force.Y;
    }

    private void _cancelObjectMove(MovingGameObject gameObject)
    {
        gameObject.X -= gameObject.Force.X;
        gameObject.Y -= gameObject.Force.Y;
    }

    private List<GameObject> _getCollisionedObjects(MovingGameObject gameObject, List<GameObject> allGameObjects)
    {
        var list = new List<GameObject>();
        foreach (var otherGameObject in allGameObjects)
            if (gameObject.CheckCollision(otherGameObject))
                list.Add(otherGameObject);
        return list;
    }

    private Vector2 _countForce(MovingGameObject gameObject)
    {
        var newForce = new Vector2();
        if (gameObject.IsOnGround)
            newForce.Y = 0;
        else
            newForce.Y = (gameObject.Force.Y + _accelerationOfFreeFall);

        newForce.X = 0;
        return newForce;
    }

    private void _callEventOnCollisionForAllCollisionedObjects(List<GameObject> gameObjects, MovingGameObject collisionedGameObject)
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.CollisionedEventHandler(collisionedGameObject);
            collisionedGameObject.CollisionedEventHandler(gameObject);
        }
    }

    private List<GameObject> _getSolidCollisionedObjects(List<GameObject> collisionedGameObjects)
    {
        var list = new List<GameObject>();
        foreach (var gameObject in collisionedGameObjects)
            if (gameObject.IsSolid)
                list.Add(gameObject);
        return list;
    }

    private void _whenObjectOnGround(MovingGameObject gameObject)
    {
        gameObject.IsOnGround = true;
        gameObject.FalledOnGroundEventHandler();
    }

    private bool _checkIfObjectIsOnGround(MovingGameObject gameObject)
    {
        if (gameObject.Y + gameObject.Height == _level.Height)
            return true;

        foreach (var otherGameObject in _level.GameObjects)
            if (otherGameObject.IsSolid)
                if (gameObject.Y + gameObject.Height == otherGameObject.Y
                    && gameObject.X + gameObject.Width >= otherGameObject.X
                    && gameObject.X <= otherGameObject.X + otherGameObject.Width)
                    return true;

        return false;
    }

    private void _normalizeObjectPosition(MovingGameObject gameObject)
    {
        if (gameObject.Y + gameObject.Height > _level.Height)
        {
            gameObject.Y = _level.Height - gameObject.Height;
            gameObject.Force.Y = 0;
        }

        if (gameObject.Y < 0)
        {
            gameObject.Force.Y = 0;
            gameObject.Y = 0;
        }

        if (gameObject.X < 0)
            gameObject.X = 0;

        if (gameObject.X + gameObject.Width > _level.Width)
            gameObject.X = _level.Width - gameObject.Width;
    }
}