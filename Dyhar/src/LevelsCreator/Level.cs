﻿using Dyhar.src.Entities;
using Dyhar.src.Physics;
using System.Collections.Generic;

namespace Dyhar.src.LevelsCreator
{
    public class Level
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public Physic physic;

        public Level(List<GameObject> gameObjects, double frictionСoefficient = 1, double accelerationOfFreeFall = 1)
        {
            this.gameObjects = gameObjects;

            physic = new Physic(frictionСoefficient, accelerationOfFreeFall);
        }
    }
}
