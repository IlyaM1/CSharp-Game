using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Dyhar.src.Utils;
using Dyhar.src.Drawing;
using Dyhar.src.Control;

namespace Dyhar.src.UI;

public class DropdownList : Widget
{
    public Rectangle Rectangle { get; private set; }
    public int X => Rectangle.X;
    public int Y => Rectangle.Y;
    public int Width => Rectangle.Width;
    public int Height => Rectangle.Height;

    public static Texture2D BackgroundSprite;
    public static Texture2D ElementsSprite;
    public static SpriteFont Font;

    public DropdownList(Rectangle rectangle, List<string> options, Action<int> actionWhenEdited)
    {
        _options = options;
        _selectedOption = options[0];
        _isOpen = false;
        _actionWhenEdited = actionWhenEdited;
        Rectangle = rectangle;
    }

    public override void UpdateEventHandler(Camera camera, InputManager control, MouseState mouseState, KeyboardState keyboardState)
    {
        _drawPosition = camera.ConvertScreenPositionToMapPosition(new Vector2(X, Y));

        if (control.IsMouseLeftDown(mouseState))
            if (Rectangle.Contains(mouseState.Position))
                control.PressLeftMouse(() => _isOpen = !_isOpen);

        if (_isOpen)
        {
            for (var i = 0; i < _options.Count; i++)
            {
                var optionBounds = new Rectangle(X, Y + Height * (i + 1), Width, Height);

                if (control.IsMouseLeftDown(mouseState))
                {
                    if (optionBounds.Contains(mouseState.Position))
                    {
                        control.PressLeftMouse(TypesUtils.EmptyFunction);
                        _selectedOption = _options[i];
                        _actionWhenEdited(i);
                        _isOpen = false;
                        break;
                    }
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(BackgroundSprite, _drawRectangle, Color.White);
        spriteBatch.DrawString(Font, _selectedOption, new Vector2(_drawPosition.X + 5, _drawPosition.Y + 5), Color.Black);

        if (_isOpen)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                spriteBatch.Draw(ElementsSprite, new Rectangle((int)_drawPosition.X, (int)(_drawPosition.Y + Height * (i + 1)), Width, Height), Color.White);
                spriteBatch.DrawString(Font, _options[i], new Vector2(_drawPosition.X + 5, _drawPosition.Y + 5 + (i + 1) * Height), Color.Black);
            }
        }
    }

    
    private readonly List<string> _options;
    private string _selectedOption;
    private bool _isOpen;
    
    private Action<int> _actionWhenEdited;

    private Vector2 _drawPosition = new Vector2(0, 0);
    private Rectangle _drawRectangle => new Rectangle((int)_drawPosition.X, (int)_drawPosition.Y, Width, Height);
}