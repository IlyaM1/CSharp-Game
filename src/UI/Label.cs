using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dyhar.src.UI;

public class Label : Widget
{
    public Rectangle Rectangle { get; private set; }
    public string Text { get; set; }
    public static SpriteFont Font { get; set; }

    public Label(string text, int startX, int startY, int width, int height)
    {
        Text = text;
        Rectangle = new Rectangle(startX, startY, width, height);
        _drawPosition = new Vector2(startX, startY);
    }

    public Label(string text, Rectangle rectangle, SpriteFont font)
    {
        Text = text;
        Rectangle = rectangle;
        Font = font;
        _drawPosition = new Vector2(rectangle.X, rectangle.Y);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Font, Text, _drawPosition, Color.White);
    }

    public override void UpdateEventHandler(Camera camera, InputManager control, MouseState mouseState, KeyboardState keyboardState)
    {
        _drawPosition = camera.ConvertScreenPositionToMapPosition(new Vector2(Rectangle.X, Rectangle.Y));
    }

    private Vector2 _drawPosition = new Vector2(0, 0);
}