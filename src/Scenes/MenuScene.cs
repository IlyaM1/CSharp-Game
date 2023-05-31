using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Dyhar.src.Control;

namespace Dyhar.src.Scenes;

public class MenuScene : Scene
{
    public MenuScene()
    {
        _menuItems = new[] { "New Game" , "Levels creator", "Quit" };
        _buttonScenes = new Dictionary<string, Type>()
        {
            ["New Game"] = typeof(GameScene),
            ["Levels creator"] = typeof(LevelCreatorScene),
            ["Quit"] = null,
        };
        _selectedItemIndex = 0;
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        _standardFont = content.Load<SpriteFont>("galleryFont");
    }

    public override void Update(GameTime gameTime)
    {
        if (_control.IsKeyDown(Keyboard.GetState(), Keys.Up))
            _control.PressButton(Keys.Up, () => _decreaseItemIndex());

        if (_control.IsKeyDown(Keyboard.GetState(), Keys.Down))
            _control.PressButton(Keys.Down, () => _increaseItemIndex());

        if (_control.IsKeyDown(Keyboard.GetState(), Keys.Enter))
            _control.PressButton(Keys.Enter, () => _selectMenuItem());

        _control.RemoveUnpressedKeys(Keyboard.GetState());
        // TODO: Make support of Mouse
        // control.UnpressLeftMouse(Mouse.GetState());
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        // Отображение всех элементов меню с подсветкой выбранного элемента
        for (int i = 0; i < _menuItems.Length; i++)
        {
            var position = new Vector2(50, 50 + i * 50);
            var color = i == _selectedItemIndex ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_standardFont, _menuItems[i], position, color);
        }

        spriteBatch.End();
    }


    private SpriteFont _standardFont;

    private int _selectedItemIndex;
    private string[] _menuItems;
    private Dictionary<string, Type> _buttonScenes;

    private InputManager _control = new InputManager();

    private void _increaseItemIndex()
    {
        if (_selectedItemIndex + 1 < _buttonScenes.Count)
            _selectedItemIndex += 1;
    }

    private void _decreaseItemIndex()
    {
        if (_selectedItemIndex - 1 >= 0)
            _selectedItemIndex -= 1;
    }

    private void _selectMenuItem()
    {
        IsDone = true;
        SceneToRun = _buttonScenes[_menuItems[_selectedItemIndex]];
    }
}