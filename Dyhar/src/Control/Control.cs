using Dyhar.src.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                player.Move(-player.Speed, 0);
            if (keyboardState.IsKeyDown(Keys.D))
                player.Move(player.Speed, 0);
            if (keyboardState.IsKeyDown(Keys.W))
                player.Move(0, -player.Speed);
            if (keyboardState.IsKeyDown(Keys.S))
                player.Move(0, player.Speed);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
    }
}
