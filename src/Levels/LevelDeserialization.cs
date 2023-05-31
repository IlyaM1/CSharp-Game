using Dyhar.src.Entities;
using System;
using System.IO;
using System.Text;

namespace Dyhar.src.Levels;

public static class LevelDeserialization
{
    public static string DeserializeLevel(Level level)
    {
        var levelString = new StringBuilder();
        levelString.AppendLine(level.Width.ToString() + " " + level.Height.ToString());

        foreach (var gameObject in level.GameObjects)
        {
            var objectString = "";
            var objectName = gameObject.GetType().Name;
            switch (objectName)
            {
                case "EarthBlock":
                    objectString = $"{objectName} {gameObject.X} {gameObject.Y} {gameObject.Width} {gameObject.Height}";
                    break;
                default:
                    objectString = $"{objectName} {gameObject.X} {gameObject.Y}";
                    break;
            }
            levelString.AppendLine(objectString);
        }

        return levelString.ToString();
    }

    public static void WriteToFile(string levelName, string levelString)
    {
        File.WriteAllText($"LevelFiles\\{levelName}.lvl", levelString);
    }
}