﻿using Dyhar.src.Entities;
using Dyhar.src.Mechanics;
using System.Collections.Generic;

namespace Dyhar.src.LevelsCreator
{
    public class Level
    {
        public List<MovingGameObject> gameObjects = new List<MovingGameObject>();
        public Physic physic;

        public Level(List<MovingGameObject> gameObjects, double frictionСoefficient = 1, double accelerationOfFreeFall = 0.5)
        {
            this.gameObjects = gameObjects;

            physic = new Physic(frictionСoefficient, accelerationOfFreeFall);
        }
    }
}
