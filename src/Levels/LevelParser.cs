using Dyhar.src.Entities.AbstractClasses;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System;
using System.Linq;
using Dyhar.src.Utils;

namespace Dyhar.src.Levels;

public static class LevelParser
{
    public static Level Parse(string levelName)
    {
        var file = new FileInfo($"LevelFiles\\{levelName}.lvl");
        var fileLines = File.ReadAllLines(file.FullName);

        var sizeStringSplitted = fileLines[0].Split(" ");
        var size = new Size(int.Parse(sizeStringSplitted[0]), int.Parse(sizeStringSplitted[1]));

        var level = new Level(new List<GameObject>(), size.Width, size.Height);

        for (var i = 1; i < fileLines.Length; i++)
        {
            var lineSplitted = fileLines[i].Split(" ");
            var type = TypesUtils.GetTypeFromString(lineSplitted[0]);
            var obj = TypesUtils.CreateObject(type, lineSplitted.Skip(1).ToArray());
            var newGameObject = Convert.ChangeType(obj, type);
            level.AddToGameObjects((GameObject)newGameObject);
        }

        return level;
    }
}
