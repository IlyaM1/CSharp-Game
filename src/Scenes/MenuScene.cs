using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Dyhar.src.Scenes;

public class MenuScene : Scene
{
    SpriteFont standardFont;

    private int selectedItemIndex;
    private string[] menuItems;

    Control.Control control = new Control.Control();

    public MenuScene()
    {
        // Определение элементов меню
        menuItems = new string[]
        {
            "New Game",
            "Load Game",
            "Settings",
            "Levels creator",
            "Quit"
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
        if (selectedItemIndex + 1 < menuItems.Length)
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
        // Выбор элемента меню
        switch (selectedItemIndex)
        {
            case 0:
                IsDone = true;
                SceneToRun = typeof(GameScene);
                break;
            case 1:
                IsDone = true;
                SceneToRun = null;
                break;
            case 2:
                IsDone = true;
                SceneToRun = null;
                break;
            case 3:
                IsDone = true;
                SceneToRun = typeof(LevelCreatorScene);
                break;
            case 4:
                IsDone = true;
                SceneToRun = null;
                break;
        }
    }
}