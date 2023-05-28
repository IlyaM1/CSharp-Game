using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace Dyhar.src.Scenes;

public class MenuScene : Scene
{
    SpriteFont standardFont;

    private int selectedItemIndex;
    private string[] menuItems;
    private Dictionary<string, Type> buttonScenes;

    Control.Control control = new Control.Control();

    public MenuScene()
    {
        menuItems = new[] { "New Game" , "Levels creator", "Quit" };
        buttonScenes = new Dictionary<string, Type>()
        {
            ["New Game"] = typeof(GameScene),
            ["Levels creator"] = typeof(LevelCreatorScene),
            ["Quit"] = null,
        };
        selectedItemIndex = 0;
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        standardFont = content.Load<SpriteFont>("galleryFont");
    }

    public override void Update(GameTime gameTime)
    {
        if (control.CanReleaseKeyBePressed(Keyboard.GetState(), Keys.Up))
            control.PressButton(Keys.Up, () => DecreaseItemIndex());

        if (control.CanReleaseKeyBePressed(Keyboard.GetState(), Keys.Down))
            control.PressButton(Keys.Down, () => IncreaseItemIndex());

        if (control.CanReleaseKeyBePressed(Keyboard.GetState(), Keys.Enter))
            control.PressButton(Keys.Enter, () => SelectMenuItem());

        control.RemoveUnpressedKeys(Keyboard.GetState());
        // TODO: Make support of Mouse
        // control.UnpressLeftMouse(Mouse.GetState());
    }

    private void IncreaseItemIndex()
    {
        if (selectedItemIndex + 1 < buttonScenes.Count)
            selectedItemIndex += 1;
    }

    private void DecreaseItemIndex()
    {
        if (selectedItemIndex - 1 >= 0)
            selectedItemIndex -= 1;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        // Отображение всех элементов меню с подсветкой выбранного элемента
        for (int i = 0; i < menuItems.Length; i++)
        {
            var position = new Vector2(50, 50 + i * 50);
            var color = i == selectedItemIndex ? Color.Yellow : Color.White;
            spriteBatch.DrawString(standardFont, menuItems[i], position, color);
        }

        spriteBatch.End();
    }

    private void SelectMenuItem()
    {
        IsDone = true;
        SceneToRun = buttonScenes[menuItems[selectedItemIndex]];
    }
}