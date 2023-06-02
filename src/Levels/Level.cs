using Dyhar.src.Entities;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Dyhar.src.Utils;

using System;
using System.Collections.Generic;

namespace Dyhar.src.Levels;

public class Level
{
    public List<GameObject> GameObjects { get; private set; }
    public Physics Physics { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public int EnemyCount => _enemyCount;

    public Level(List<GameObject> gameObjects)
    {
        GameObjects = gameObjects;

        foreach (var gameObject in GameObjects)
            if (gameObject is Enemy)
                _enemyCount++;

        Physics = new Physics(this);

        Width = 2000;
        Height = 1200;
    }

    public Level(List<GameObject> gameObjects, int width, int height) : this(gameObjects) 
    {
        Width = width;
        Height = height;
    }

    public static Level CreateLevelFromFile(string fileName, Control.GameControl control, out Player player)
    {
        Level parsedLevel = LevelParser.Parse(fileName);

        Player playerObject = null;
        foreach (var gameObject in parsedLevel.GameObjects)
            if (gameObject is Player)
                playerObject = gameObject as Player;

        if (playerObject is null)
            throw new InvalidOperationException("There are no player on level");

        player = playerObject;
        control.SetPlayer(playerObject);

        return parsedLevel;
    }

    public static Level CreateLevelFromFile(string fileName)
    {
        return LevelParser.Parse(fileName);
    }

    public void AddToGameObjects(GameObject gameObject)
    {
        if (gameObject == null)
            throw new ArgumentNullException($"{gameObject} that you want to add in game objects is null");
        GameObjects.Add(gameObject);
        if (gameObject is Enemy)
            _enemyCount++;
    }

    public void RemoveFromGameObjects(GameObject gameObject)
    {
        GameObjects.Remove(gameObject);
        if (gameObject is Enemy)
            _enemyCount--;
    }

    public List<IWeaponUser> GetWeaponUsers()
    {
        var result = new List<IWeaponUser>();
        foreach (var gameObject in GameObjects)
            if (gameObject is IWeaponUser weaponUser)
                result.Add(weaponUser);
        return result;
    }

    private int _enemyCount = 0;
}
