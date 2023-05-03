using Dyhar.src.Entities;
using Microsoft.Xna.Framework.Input;
using System;
using Dyhar.src.Mechanics;
using System.Collections.Generic;

namespace Dyhar.src.Control
{
    public enum ControlState
    {
        Menu,
        Game,
        Interface
    }

    public sealed class Control
    {
        ControlState State { get; set; }
        Player player;

        List<Keys> pressedKeys = new List<Keys>();
        bool isPressedLeftMouse = false;

        public Control(ControlState state)
        {
            State = state;
        }

        public void onUpdate(MouseState mouseState, KeyboardState keyboardState)
        {
            if (State == ControlState.Game)
                onGameUpdate(mouseState, keyboardState);
        }

        public void onGameUpdate(MouseState mouseState, KeyboardState keyboardState)
        {
            if (player == null)
                throw new Exception("Player isn't setted in control");

            if (keyboardState.IsKeyDown(Keys.A))
                PressButton(Keys.A, () => player.MoveHorizontally(Direction.Left));
            if (keyboardState.IsKeyDown(Keys.D))
                PressButton(Keys.D, () => player.MoveHorizontally(Direction.Right));
            if (keyboardState.IsKeyDown(Keys.W) && !pressedKeys.Contains(Keys.W))
                PressButton(Keys.W, () => player.Jump());

            if (mouseState.LeftButton == ButtonState.Pressed && !isPressedLeftMouse)
                PressLeftMouse(() => player.onAttack());

            RemoveUnpressedKeys(keyboardState);
            UnpressLeftMouse(mouseState);
        }

        private void PressButton(Keys key, Action action)
        {
            pressedKeys.Add(key);
            action();
        }

        private void PressLeftMouse(Action action)
        {
            isPressedLeftMouse = true;
            action();
        }

        private void RemoveUnpressedKeys(KeyboardState keyboardState)
        {
            var unpressedKeys = new List<Keys>();
            foreach (var key in pressedKeys)
                if (keyboardState.IsKeyUp(key))
                    unpressedKeys.Add(key);

            foreach(var key in unpressedKeys)
                pressedKeys.Remove(key);
        }

        private void UnpressLeftMouse(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Released)
                isPressedLeftMouse = false;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
    }
}
