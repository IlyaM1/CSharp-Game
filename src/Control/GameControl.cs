using Dyhar.src.Entities;
using Microsoft.Xna.Framework.Input;
using System;
using Dyhar.src.Mechanics;
using System.Collections.Generic;
using Dyhar.src.Drawing;

namespace Dyhar.src.Control
{
    public sealed class GameControl
    {
        Player player;
        Camera camera;

        Control control = new Control();

        public GameControl() { }

        public GameControl(Player player) : this()
        {
            this.player = player;
        }

        public GameControl(Camera camera)
        {
            this.camera = camera;
        }

        public GameControl(Player player, Camera camera) : this(player)
        {
            this.camera = camera;
        }

        public void onUpdate(MouseState mouseState, KeyboardState keyboardState)
        {
            if (player == null)
                throw new Exception("Player isn't setted in control");

            if (control.CanPressKeyBePressed(keyboardState, Keys.A))
                control.PressButton(Keys.A, () => player.MoveHorizontally(Direction.Left));

            if (control.CanPressKeyBePressed(keyboardState, Keys.D))
                control.PressButton(Keys.D, () => player.MoveHorizontally(Direction.Right));

            if (control.CanReleaseKeyBePressed(keyboardState, Keys.W))
                control.PressButton(Keys.W, () => player.Jump());

            if (control.CanReleaseKeyBePressed(keyboardState, Keys.LeftShift))
                control.PressButton(Keys.LeftShift, () => player.Dash(camera));


            if (mouseState.LeftButton == ButtonState.Pressed && !control.IsPressedLeftMouse)
                control.PressLeftMouse(() => player.onAttack());

            control.RemoveUnpressedKeys(keyboardState);
            control.UnpressLeftMouse(mouseState);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void setCamera(Camera camera)
        {
            this.camera = camera;
        }
    }
}
