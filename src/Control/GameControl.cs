using Dyhar.src.Entities;
using Microsoft.Xna.Framework.Input;
using System;
using Dyhar.src.Mechanics;
using Dyhar.src.Drawing;
using Dyhar.src.Scenes;

namespace Dyhar.src.Control;

public class GameControl
{
    private Player _player;
    private Camera _camera;

    private InputManager _control = new InputManager();

    public void Update(MouseState mouseState, KeyboardState keyboardState)
    {
        if (_player == null)
            throw new Exception("Player isn't setted in control");

        if (_control.IsKeyHold(keyboardState, Keys.A))
            _control.PressButton(Keys.A, () => _player.MoveHorizontally(Direction.Left));

        if (_control.IsKeyHold(keyboardState, Keys.D))
            _control.PressButton(Keys.D, () => _player.MoveHorizontally(Direction.Right));

        if (_control.IsKeyDown(keyboardState, Keys.W))
            _control.PressButton(Keys.W, () => _player.Jump());

        if (_control.IsKeyDown(keyboardState, Keys.LeftShift))
            _control.PressButton(Keys.LeftShift, () => _player.Dash(_camera));

        if (_control.IsMouseLeftDown(mouseState))
            _control.PressLeftMouse(() => _player.AttackingEventHandler());


        if (_control.IsKeyDown(keyboardState, Keys.R))
            _control.PressButton(Keys.R, () => _rerunNewLevel());


        _control.RemoveUnpressedKeys(keyboardState);
        _control.UnpressLeftMouse(mouseState);
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    private void _rerunNewLevel()
    {
        var dyharInstance = Dyhar.GetInstance();
        dyharInstance.CurrentScene.RunNewScene(typeof(GameScene));
    }
}
