using Dyhar.src.Drawing;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Entities;
using Dyhar.src.Utils;
using Dyhar.src.Control;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using Dyhar.src.UI;
using System.IO;

namespace Dyhar.src.Scenes;

internal enum LevelObjects
{
    Player,
    EarthBlock,
    Swordsman
}

internal enum Mode
{
    Creating,
    Deleting
}

public class LevelCreatorScene : Scene
{
    Level.Level currentLevel;
    Camera camera;
    Texture2D _gridCube;
    SpriteFont standardFont;
    float cameraX = 800.0f;
    float cameraY = 450.0f;
    Vector2 cameraPosition => new Vector2(cameraX, cameraY);

    Control.Control control = new Control.Control();

    List<Widget> widgets = new List<Widget>();
    Texture2D dropdownMenuSprite;
    Texture2D dropdownMenuElementSprite;

    List<string> levelNames;
    int gridSize = 50;

    LevelObjects currentGameObject = LevelObjects.Swordsman;
    Mode currentMode = Mode.Creating;

    public LevelCreatorScene()
    {
        // currentLevel = new Level.Level(new List<GameObject>(), 1600, 900);
        currentLevel = Level.Level.CreateLevelFromFile("Level1");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        camera.Update(cameraPosition);
        spriteBatch.Begin(transformMatrix: camera.Transform);

        foreach (var widget in widgets)
            widget.Draw(spriteBatch);

        DrawLevel(spriteBatch);

        spriteBatch.End();
    }

    public void DrawLevel(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < currentLevel.GameObjects.Count; i++)
        {
            var gameObject = currentLevel.GameObjects[i];
            gameObject.Draw(spriteBatch);

            if (TypesUtils.CanBeDownCasted<GameObject, IWeaponUser>(gameObject))
            {
                var weaponUser = (IWeaponUser)gameObject;
                if (weaponUser.IsAttacking())
                    weaponUser.GetCurrentWeapon().Draw(spriteBatch);
            }
        }

        for (var i = 0; i < currentLevel.Width; i += 50)
            for (var j = 0; j < currentLevel.Height; j += 50)
                spriteBatch.Draw(_gridCube, new Rectangle(i, j, 50, 50), Color.White);
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        Player.sprite = content.Load<Texture2D>("player");
        EarthBlock.sprite = content.Load<Texture2D>("Earth2");
        Sword.sprite = content.Load<Texture2D>("Sword");
        Swordsman.sprite = content.Load<Texture2D>("Swordsman");
        _gridCube = content.Load<Texture2D>("GridCube1");

        standardFont = content.Load<SpriteFont>("galleryFont");

        dropdownMenuSprite = content.Load<Texture2D>("DropdownWidget");
        dropdownMenuElementSprite = content.Load<Texture2D>("DropdownWidgetElement");

        camera = new Camera(graphics.Viewport, currentLevel.Width, currentLevel.Height);

        CreateAllWidgets();
    }

    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            IsDone = true;
            SceneToRun = typeof(MenuScene);
        }

        // TODO: make object changing, adding, deleting etc

        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();

        MoveCameraUpdate(keyboardState);

        foreach (var widget in widgets)
            widget.Update(camera, control, mouseState, keyboardState);

        if (control.CanReleaseLeftMouseBePressed(Mouse.GetState()))
        {
            if (currentMode == Mode.Creating)
                control.PressLeftMouse(() => CreateModeAction(mouseState));
            if (currentMode == Mode.Deleting)
                control.PressLeftMouse(() => DeleteModeAction(mouseState));
        }

        control.RemoveUnpressedKeys(keyboardState);
        control.UnpressLeftMouse(mouseState);
    }

    private void MoveCameraUpdate(KeyboardState keyboardState)
    {
        if (control.CanPressKeyBePressed(keyboardState, Keys.A))
            control.PressButton(Keys.A, () => cameraX -= 20);

        if (control.CanPressKeyBePressed(keyboardState, Keys.D))
            control.PressButton(Keys.D, () => cameraX += 20);

        if (control.CanPressKeyBePressed(keyboardState, Keys.W))
            control.PressButton(Keys.W, () => cameraY -= 20);

        if (control.CanPressKeyBePressed(keyboardState, Keys.S))
            control.PressButton(Keys.S, () => cameraY += 20);

        cameraX = MathHelper.Clamp(cameraX, 800, currentLevel.Width - 800);
        cameraY = MathHelper.Clamp(cameraY, 450, currentLevel.Height - 450);
    }

    private void CreateAllWidgets()
    {
        levelNames = GetAllLevelsNames();
        levelNames.Add("New level");

        var levelChoose = new DropdownList(new Rectangle(0, 0, 200, 50),
            dropdownMenuSprite,
            dropdownMenuElementSprite,
            standardFont,
            levelNames,
            x => ChangeLevel(x));
        widgets.Add(levelChoose);
    }

    private List<string> GetAllLevelsNames()
    {
        var filePaths = Directory.GetFiles("LevelFiles");
        var result = new List<string>();

        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            result.Add(fileName);
        }

        return result;
    }

    private void ChangeLevel(int levelIndex)
    {
        if (levelIndex == levelNames.Count - 1)
            CreateNewLevel();
        else
            ChangeExistingLevel(levelNames[levelIndex]);
    }

    private void CreateNewLevel()
    {
        currentLevel = new Level.Level(new List<GameObject>());
        camera.SetNewMapSize(new System.Drawing.Size(currentLevel.Width, currentLevel.Height));
    }

    private void ChangeExistingLevel(string levelName)
    {
        currentLevel = Level.Level.CreateLevelFromFile(levelName);
        camera.SetNewMapSize(new System.Drawing.Size(currentLevel.Width, currentLevel.Height));
    }

    private Vector2 GetGridCellIndex(int x, int y)
    {
        return new Vector2(x / gridSize, y / gridSize);
    }

    private void CreateModeAction(MouseState mouseState)
    {
        var gridCell = GetGridCellIndex((int)(mouseState.X + cameraX - 800), (int)(mouseState.Y + cameraY - 450));
        var coordinates = new Vector2(gridCell.X * gridSize, gridCell.Y * gridSize);
        var coordinatesInStrings = new string[] { coordinates.X.ToString(), coordinates.Y.ToString() };
        var objectType = TypesUtils.GetTypeFromString(currentGameObject.ToString());
        if (currentGameObject == LevelObjects.EarthBlock)
        {
            // TODO: make for EarthBlock
        }
        else
        {
            var newGameObject = (GameObject)TypesUtils.CreateObject(objectType, coordinatesInStrings);
            currentLevel.AddToGameObjects(newGameObject);
        }
    }

    private void DeleteModeAction(MouseState mouseState)
    {

    }
}