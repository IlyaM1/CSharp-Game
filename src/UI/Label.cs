using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dyhar.src.UI;

public class Label : Widget
{
    public string Text { get; set; }

    public Label(string text, int startX, int startY, int width, int height, SpriteFont font)
    {
        Text = text;
        _rectangle = new Rectangle(startX, startY, width, height);
        _font = font;
        _drawPosition = new Vector2(startX, startY);
    }

    public Label(string text, Rectangle rectangle, SpriteFont font)
    {
        Text = text;
        _rectangle = rectangle;
        _font = font;
        _drawPosition = new Vector2(rectangle.X, rectangle.Y);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, Text, _drawPosition, Color.White);
    }

    public override void Update(Camera camera, InputManager control, MouseState mouseState, KeyboardState keyboardState)
    {
        _drawPosition = camera.ConvertScreenPositionToMapPosition(new Vector2(_rectangle.X, _rectangle.Y));
    }

    private Rectangle _rectangle { get; set; }
    private SpriteFont _font { get; set; }
    private Vector2 _drawPosition = new Vector2(0, 0);
}