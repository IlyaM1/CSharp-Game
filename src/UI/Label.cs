using Dyhar.src.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dyhar.src.UI;

public class Label : Widget
{
    public string Text { get; set; }
    Rectangle Rectangle { get; set; }
    SpriteFont Font { get; set; }
    Vector2 Position => new Vector2(Rectangle.X, Rectangle.Y);

    public Label(string text, int startX, int startY, int width, int height, SpriteFont font)
    {
        Text = text;
        Rectangle = new Rectangle(startX, startY, width, height);
        Font = font;
    }

    public Label(string text, Rectangle rectangle, SpriteFont font)
    {
        Text = text;
        Rectangle = rectangle;
        Font = font;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Font, Text, Position, Color.White);
    }

    public override void Update(Camera camera, Control.Control control, MouseState mouseState, KeyboardState keyboardState) { }
}
