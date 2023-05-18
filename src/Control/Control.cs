﻿using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Dyhar.src.Control;

public class Control
{
    public List<Keys> PressedKeys { get; set; }
    public bool IsPressedLeftMouse { get; set; }

    public Control()
    {
        PressedKeys = new List<Keys>();
        IsPressedLeftMouse = false;
    }

    public bool CanReleaseKeyBePressed(KeyboardState keyboardState, Keys key)
    {
        if (keyboardState.IsKeyDown(key) && !PressedKeys.Contains(key))
            return true;
        return false;
    }

    public bool CanPressKeyBePressed(KeyboardState keyboardState, Keys key)
    {
        if (keyboardState.IsKeyDown(key))
            return true;
        return false;
    }

    public void PressButton(Keys key, Action action)
    {
        PressedKeys.Add(key);
        action();
    }

    public void PressLeftMouse(Action action)
    {
        IsPressedLeftMouse = true;
        action();
    }

    public void RemoveUnpressedKeys(KeyboardState keyboardState)
    {
        var unpressedKeys = new List<Keys>();
        foreach (var key in PressedKeys)
            if (keyboardState.IsKeyUp(key))
                unpressedKeys.Add(key);

        foreach (var key in unpressedKeys)
            PressedKeys.Remove(key);
    }

    public void UnpressLeftMouse(MouseState mouseState)
    {
        if (mouseState.LeftButton == ButtonState.Released)
            IsPressedLeftMouse = false;
    }
}
