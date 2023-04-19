using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Dyhar.src.Entities;

public abstract class GameObject
{
    public double X { get; set; }
    public double Y { get; set; }
    public Vector2 Position { get => new Vector2((int)X, (int)Y); }
    public Size SizeSprite { get => new Size(GetSprite().Width, GetSprite().Height); }


    public abstract void onUpdate(GameTime gameTime);
    public abstract Texture2D GetSprite();
}