using Dyhar.src.Entities;
using Microsoft.Xna.Framework.Input;
using System;
using Dyhar.src.Physics;

namespace Dyhar.src.Control
{
    public enum ControlState
    {
        Menu,
        Game,
        Interface
    }

    public class Control
    {
        ControlState State { get; set; }
        Player player;

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
                player.MoveHorizontally(Direction.Left);
            if (keyboardState.IsKeyDown(Keys.D))
                player.MoveHorizontally(Direction.Right);
            if (keyboardState.IsKeyDown(Keys.W))
                player.Jump();
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
    }
}
