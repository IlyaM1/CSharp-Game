using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using System.Collections.Generic;

namespace Dyhar.src.LevelsCreator
{
    public class Level
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<IWeaponUser> weaponUsers = new List<IWeaponUser>();
        public Physic physic;

        public int Width { get; set; }
        public int Height { get; set; }

        public Level(List<GameObject> gameObjects, double frictionСoefficient = 1, double accelerationOfFreeFall = 0.5)
        {
            this.gameObjects = gameObjects;

            physic = new Physic(this);

            Width = 2000;
            Height = 1200;
        }
    }
}
