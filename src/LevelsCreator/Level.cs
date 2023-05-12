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
        public List<GameObject> GameObjects { get; set; }
        public List<IWeaponUser> weaponUsers { get; set; }
        public Physics Physics { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Level(List<GameObject> gameObjects, List<IWeaponUser> weaponUsers)
        {
            this.GameObjects = gameObjects;
            this.weaponUsers = weaponUsers;

            Physics = new Physics(this);

            Width = 2000;
            Height = 1200;
        }

        public static Level CreateTestLevel(Player player)
        {
            var currentLevel = new Level(new[] { (GameObject)player }.ToList(), new[] { (IWeaponUser)player}.ToList());

            currentLevel.GameObjects.Add(new EarthBlock(250, 600, 250, 50));
            currentLevel.GameObjects.Add(new EarthBlock(250, 1000, 250, 200));

            return currentLevel;
        }
    }
}
