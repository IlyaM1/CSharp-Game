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
    public int X => _rectangle.X;
    public int Y => _rectangle.Y;
    public int Width => _rectangle.Width;
    public int Height => _rectangle.Height;

    public DropdownList(Rectangle rectangle, Texture2D backgroundSprite, Texture2D elementsSprite,
        SpriteFont font, List<string> options, Action<int> actionWhenEdited)
    {
        _font = font;
        _options = options;
        _selectedOption = options[0];
        _isOpen = false;
        _rectangle = rectangle;
        _backgroundSprite = backgroundSprite;
        _actionWhenEdited = actionWhenEdited;
        _elementsSprite = elementsSprite;
    }

    public override void Update(Camera camera, InputManager control, MouseState mouseState, KeyboardState keyboardState)
    {
        _drawPosition = camera.ConvertScreenPositionToMapPosition(new Vector2(X, Y));

        if (control.IsMouseLeftDown(mouseState))
            if (_rectangle.Contains(mouseState.Position))
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
        spriteBatch.Draw(_backgroundSprite, _drawRectangle, Color.White);
        spriteBatch.DrawString(_font, _selectedOption, new Vector2(_drawPosition.X + 5, _drawPosition.Y + 5), Color.Black);

        if (_isOpen)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                spriteBatch.Draw(_elementsSprite, new Rectangle((int)_drawPosition.X, (int)(_drawPosition.Y + Height * (i + 1)), Width, Height), Color.White);
                spriteBatch.DrawString(_font, _options[i], new Vector2(_drawPosition.X + 5, _drawPosition.Y + 5 + (i + 1) * Height), Color.Black);
            }
        }
    }

    private readonly SpriteFont _font;
    private readonly List<string> _options;
    private string _selectedOption;
    private bool _isOpen;
    private Texture2D _backgroundSprite;
    private Texture2D _elementsSprite;
    private Action<int> _actionWhenEdited;

    private Rectangle _rectangle;
    private Vector2 _drawPosition = new Vector2(0, 0);
    private Rectangle _drawRectangle => new Rectangle((int)_drawPosition.X, (int)_drawPosition.Y, Width, Height);
}