using Dyhar.src.Entities;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using System.Collections.Generic;
using System.Linq;

namespace Dyhar.src.LevelsCreator
{
    public class Level
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<IWeaponUser> weaponUsers = new List<IWeaponUser>();
        public Physics physic;

        public int Width { get; set; }
        public int Height { get; set; }

        public Level(List<GameObject> gameObjects, double frictionСoefficient = 1, double accelerationOfFreeFall = 0.5)
        {
            this.gameObjects = gameObjects;

            physic = new Physics(this);

            Width = 2000;
            Height = 1200;
        }

        public static Level CreateTestLevel(Player player)
        {
            var currentLevel = new Level(new[] { (GameObject)player }.ToList());
            currentLevel.weaponUsers.Add(player);

            currentLevel.gameObjects.Add(new EarthBlock(250, 600, 250, 50));
            currentLevel.gameObjects.Add(new EarthBlock(250, 1000, 250, 200));

            return currentLevel;
        }
    }
}
