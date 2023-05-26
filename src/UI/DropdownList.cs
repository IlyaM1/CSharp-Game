using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Dyhar.src.Entities;
using Dyhar.src.Utils;
using Dyhar.src.Drawing;

namespace Dyhar.src.UI;

public class DropdownList : Widget
{
    private readonly SpriteFont _font;
    private readonly List<string> _options;
    private string _selectedOption;
    private bool _isOpen;
    Texture2D _backgroundSprite;
    Texture2D _elementsSprite;

    Rectangle Rectangle;

    public int X => Rectangle.X;
    public int Y => Rectangle.Y;
    public int Width => Rectangle.Width;
    public int Height => Rectangle.Height;

    private Vector2 drawPosition = new Vector2(0, 0);
    private Rectangle drawRectangle => new Rectangle((int)drawPosition.X, (int)drawPosition.Y, Width, Height);

    Action<int> _actionWhenEdited;

    public DropdownList(Rectangle rectangle, Texture2D backgroundSprite, Texture2D elementsSprite,
        SpriteFont font, List<string> options, Action<int> actionWhenEdited)
    {
        _font = font;
        _options = options;
        _selectedOption = options[0];
        _isOpen = false;
        Rectangle = rectangle;
        _backgroundSprite = backgroundSprite;
        _actionWhenEdited = actionWhenEdited;
        _elementsSprite = elementsSprite;
    }

    public override void Update(Camera camera, Control.Control control, MouseState mouseState, KeyboardState keyboardState)
    {
        drawPosition = camera.ScreenPositionToMapPosition(new Vector2(X, Y));

        // Open the list if clicked
        if (control.CanReleaseLeftMouseBePressed(mouseState))
            if (Rectangle.Contains(mouseState.Position))
                control.PressLeftMouse(() => _isOpen = !_isOpen);

        // Select an element if clicked
        if (_isOpen)
        {
            for (var i = 0; i < _options.Count; i++)
            {
                var optionBounds = new Rectangle(X, Y + Height * (i + 1), Width, Height);

                if (control.CanReleaseLeftMouseBePressed(mouseState))
                {
                    if (optionBounds.Contains(mouseState.Position))
                    {
                        control.PressLeftMouse(TypesUtils.EmptyFunction);
                        _selectedOption = _options[i];
                        _actionWhenEdited(i);
                        _isOpen = false;
                        break; // Exit the loop if an option is selected
                    }
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Draw the selected option
        spriteBatch.Draw(_backgroundSprite, drawRectangle, Color.White);
        spriteBatch.DrawString(_font, _selectedOption, new Vector2(drawPosition.X + 5, drawPosition.Y + 5), Color.Black);

        // Draw the options if the list is open
        if (_isOpen)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                spriteBatch.Draw(_elementsSprite, new Rectangle((int)drawPosition.X, (int)(drawPosition.Y + Height * (i + 1)), Width, Height), Color.White);
                spriteBatch.DrawString(_font, _options[i], new Vector2(drawPosition.X + 5, drawPosition.Y + 5 + (i + 1) * Height), Color.Black);
            }
        }
    }
}
