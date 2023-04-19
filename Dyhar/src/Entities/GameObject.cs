using Microsoft.Xna.Framework;
using System.Drawing;

namespace Dyhar.src.Entities;

public abstract class GameObject
{
    public double X { get; set; }
    public double Y { get; set; }
    public Vector2 Position { get => new Vector2((int)X, (int)Y); }

    public Size SizeSprite;

    public abstract void onUpdate(GameTime gameTime);
}