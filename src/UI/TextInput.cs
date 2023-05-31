using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dyhar.src.UI;

public class NumberTextInput : Widget
{
    public NumberTextInput(Rectangle rectangle, int hintText, int maxLength, SpriteFont font, Action<int> whenLostFocus)
    {
        _rectangle = rectangle;
        _hintText = hintText.ToString();
        _text = hintText.ToString();
        _maxLength = maxLength;
        _font = font;
        _lostFocus = whenLostFocus;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var color = Color.Gray;
        if (HasFocus())
            color = Color.Black;

        spriteBatch.DrawString(_font, _text, _drawPosition, color);
        if (string.IsNullOrEmpty(_text))
            spriteBatch.DrawString(_font, _hintText, _drawPosition, Color.Gray);
    }

    public override void Update(Camera camera, InputManager control, MouseState mouseState, KeyboardState keyboardState)
    {
        _drawPosition = camera.ConvertScreenPositionToMapPosition(new Vector2(_rectangle.X, _rectangle.Y));

        if (HasFocus())
        {
            if (!_isInFocusContinuos)
                _isInFocusContinuos = true;
            // TODO: make cleaning from start if after focus
            foreach (var key in keyboardState.GetPressedKeys())
            {
                if (control.IsKeyDown(keyboardState, key))
                {
                    if (key >= Keys.D0 && key <= Keys.D9)
                    {
                        if (_text == "0")
                            control.PressButton(key, () => _text = key.ToString()[1].ToString());
                        else
                            if (_text.Length < _maxLength)
                                control.PressButton(key, () => _text += key.ToString()[1]);
                    }
                    else if (key == Keys.Back && _text.Length > 0)
                    {
                        control.PressButton(key, () => _text = _text.Remove(_text.Length - 1));
                    }
                }
            }
        }
        else
        {
            if (_isInFocusContinuos)
            {
                _isInFocusContinuos = false;
                _lostFocus(GetValue());
            } 
        }  
    }

    private bool HasFocus()
    {
        return _rectangle.Contains(Mouse.GetState().Position);
    }

    public int GetValue()
    {
        return int.Parse(_text);
    }

    public void SetValue(int value)
    {
        if (value >= 0 && value <= Math.Pow(10, _maxLength))
            _text = value.ToString();
    }


    private readonly string _hintText;
    private readonly SpriteFont _font;
    private readonly Rectangle _rectangle;
    private readonly int _maxLength;
    private string _text = "0";

    private Action<int> _lostFocus;

    private Vector2 _drawPosition = new Vector2(0, 0);
    private bool _isInFocusContinuos = false;
}