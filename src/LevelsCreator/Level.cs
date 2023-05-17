using Dyhar.src.Entities;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Dyhar.src.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Dyhar.src.LevelsCreator;

public class Level
{
    public List<GameObject> GameObjects { get; set; }
    public Physics Physics { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Level(List<GameObject> gameObjects)
    {
        GameObjects = gameObjects;

        Physics = new Physics(this);

        Width = 2000;
        Height = 1200;
    }

    public static Level CreateTestLevel(Control.Control control, out Player player)
    {
        var playerObject = new Player(0, 0);
        player = playerObject;
        control.SetPlayer(playerObject);

        var currentLevel = new Level(new[] { (GameObject)player }.ToList());

        currentLevel.GameObjects.Add(new EarthBlock(250, 600, 250, 50));
        currentLevel.GameObjects.Add(new EarthBlock(250, 1000, 250, 200));

        currentLevel.GameObjects.Add(new Swordsman(700, 50));
        currentLevel.GameObjects.Add(new Swordsman(400, 120));
        currentLevel.GameObjects.Add(new Swordsman(100, 50));
        currentLevel.GameObjects.Add(new Swordsman(500, 50));
        currentLevel.GameObjects.Add(new Swordsman(1000, 600));
        

        return currentLevel;
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
