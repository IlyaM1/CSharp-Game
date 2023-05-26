using Dyhar.src.Entities;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Dyhar.src.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Dyhar.src.Level;

public class Level
{
    public List<GameObject> GameObjects { get; set; }
    public Physics Physics { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public int EnemyCount { get {
            var result = 0;
            foreach (var obj in GameObjects)
                if (TypesUtils.CanBeDownCasted<GameObject, Enemy>(obj))
                    result += 1;
            return result; 
        }
    }

    public Level(List<GameObject> gameObjects)
    {
        GameObjects = gameObjects;

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
                playerObject = (Player)gameObject;
            

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
    }

    public List<IWeaponUser> GetWeaponUsers()
    {
        var result = new List<IWeaponUser>();
        foreach (var gameObject in GameObjects)
            if (TypesUtils.CanBeDownCasted<GameObject, IWeaponUser>(gameObject))
                result.Add((IWeaponUser)gameObject);
        return result;
    }
}
