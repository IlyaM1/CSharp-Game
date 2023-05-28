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

    private Vector2 drawPosition = new Vector2(0, 0);

    public Label(string text, int startX, int startY, int width, int height, SpriteFont font)
    {
        Text = text;
        Rectangle = new Rectangle(startX, startY, width, height);
        Font = font;
        drawPosition = new Vector2(startX, startY);
    }

    public Label(string text, Rectangle rectangle, SpriteFont font)
    {
        Text = text;
        Rectangle = rectangle;
        Font = font;
        drawPosition = new Vector2(rectangle.X, rectangle.Y);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Font, Text, drawPosition, Color.White);
    }

    public override void Update(Camera camera, Control.Control control, MouseState mouseState, KeyboardState keyboardState)
    {
        drawPosition = camera.ScreenPositionToMapPosition(new Vector2(Rectangle.X, Rectangle.Y));
    }
}