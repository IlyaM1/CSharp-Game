using Dyhar.src.Drawing;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Entities;
using Dyhar.src.Utils;
using Dyhar.src.Levels;
using Dyhar.src.Control;
using Dyhar.src.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

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
    public LevelCreatorScene()
    {
        currentLevel = Level.CreateLevelFromFile(levelName);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: camera.Transform);

        _drawLevel(spriteBatch);

        foreach (var widget in widgets)
            widget.Draw(spriteBatch);

        spriteBatch.End();
        camera.Update(cameraPosition);
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        Player.Sprite = content.Load<Texture2D>("player");
        EarthBlock.Sprite = content.Load<Texture2D>("Earth2");
        Sword.Sprite = content.Load<Texture2D>("Sword");
        Swordsman.Sprite = content.Load<Texture2D>("Swordsman");
        _gridCube = content.Load<Texture2D>("GridCube1");

        standardFont = content.Load<SpriteFont>("galleryFont");

        dropdownMenuSprite = content.Load<Texture2D>("DropdownWidget");
        dropdownMenuElementSprite = content.Load<Texture2D>("DropdownWidgetElement");

        camera = new Camera(graphics.Viewport, currentLevel.Width, currentLevel.Height);

        _createAllWidgets();
    }

    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            IsDone = true;
            SceneToRun = typeof(MenuScene);
            var levelString = LevelDeserialization.DeserializeLevel(currentLevel);
            if (levelName == "New level")
                levelName = "Level" + (levelNames.Count()).ToString();
            if (_checkIfPlayerAlreadyExists() && currentLevel.EnemyCount > 0)
                LevelDeserialization.WriteToFile(levelName, levelString.ToString());
        }

        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();

        foreach (var widget in widgets)
            widget.Update(camera, control, mouseState, keyboardState);

        _moveCameraUpdate(keyboardState);

        if (control.IsMouseLeftDown(Mouse.GetState()))
        {
            if (currentMode == Mode.Creating)
                control.PressLeftMouse(() => _createModeAction(mouseState));
            if (currentMode == Mode.Deleting)
                control.PressLeftMouse(() => _deleteModeAction(mouseState));
        }

        control.RemoveUnpressedKeys(keyboardState);
        control.UnpressLeftMouse(mouseState);
    }



    private Level currentLevel;
    private string levelName = "Level1";

    private Camera camera;
    private Texture2D _gridCube;
    private SpriteFont standardFont;
    private float cameraX = Resolution.EtalonWidth / 2;
    private float cameraY = Resolution.EtalonHeight / 2;
    private Vector2 cameraPosition => new Vector2(cameraX, cameraY);

    private InputManager control = new InputManager();

    private List<Widget> widgets = new List<Widget>();
    private Texture2D dropdownMenuSprite;
    private Texture2D dropdownMenuElementSprite;

    private List<string> levelNames;
    private int gridSize = 50;

    private LevelObjects currentGameObject = LevelObjects.Player;
    private Mode currentMode = Mode.Creating;

    private NumberTextInput widthInput;
    private NumberTextInput heightInput;

    private Vector2 earthBlockCoordinates = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

    private void _drawLevel(SpriteBatch spriteBatch)
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

        for (var i = 0; i < currentLevel.Width; i += gridSize)
            for (var j = 0; j < currentLevel.Height; j += gridSize)
                spriteBatch.Draw(_gridCube, new Rectangle(i, j, gridSize, gridSize), Color.White);

        if (earthBlockCoordinates.X != float.NegativeInfinity && earthBlockCoordinates.Y != float.NegativeInfinity)
        {
            var mouse = Mouse.GetState();
            var mouseInMapCoordinates = camera.ConvertScreenPositionToMapPosition(new Vector2(mouse.X, mouse.Y));

            for (var i = earthBlockCoordinates.X; i < mouseInMapCoordinates.X; i += gridSize)
                for (var j = earthBlockCoordinates.Y; j < mouseInMapCoordinates.Y; j += gridSize)
                    spriteBatch.Draw(EarthBlock.Sprite, new Rectangle((int)i, (int)j, gridSize, gridSize), Color.Gray);
        }
    }

    private void _moveCameraUpdate(KeyboardState keyboardState)
    {
        if (control.IsKeyHold(keyboardState, Keys.A))
            control.PressButton(Keys.A, () => cameraX -= 20);

        if (control.IsKeyHold(keyboardState, Keys.D))
            control.PressButton(Keys.D, () => cameraX += 20);

        if (control.IsKeyHold(keyboardState, Keys.W))
            control.PressButton(Keys.W, () => cameraY -= 20);

        if (control.IsKeyHold(keyboardState, Keys.S))
            control.PressButton(Keys.S, () => cameraY += 20);

        cameraX = MathHelper.Clamp(cameraX, (Resolution.EtalonWidth / 2), currentLevel.Width - (Resolution.EtalonWidth / 2));
        cameraY = MathHelper.Clamp(cameraY, (Resolution.EtalonHeight / 2), currentLevel.Height - (Resolution.EtalonHeight / 2));
    }

    private void _createAllWidgets()
    {
        levelNames = _getAllLevelsNames();
        levelNames.Add("New level");

        var levelChoose = new DropdownList(new Rectangle(0, 0, 200, 50),
            dropdownMenuSprite,
            dropdownMenuElementSprite,
            standardFont,
            levelNames,
            x => _changeLevel(x));
        widgets.Add(levelChoose);

        var allGameObjectsNames = typeof(LevelObjects).GetEnumNames();
        var levelObjectChoose = new DropdownList(new Rectangle(200, 0, 200, 50),
            dropdownMenuSprite,
            dropdownMenuElementSprite,
            standardFont,
            allGameObjectsNames.ToList(),
            x => currentGameObject = (LevelObjects)Enum.Parse(typeof(LevelObjects), allGameObjectsNames[x]));
        widgets.Add(levelObjectChoose);

        var allModesNames = typeof(Mode).GetEnumNames();
        var modeChoose = new DropdownList(new Rectangle(400, 0, 200, 50),
            dropdownMenuSprite,
            dropdownMenuElementSprite,
            standardFont,
            allModesNames.ToList(),
            x => currentMode = (Mode)Enum.Parse(typeof(Mode), allModesNames[x]));
        widgets.Add(modeChoose);

        widthInput = new NumberTextInput(new Rectangle(600, 0, 200, 50),
            currentLevel.Width,
            5,
            standardFont,
            _changeLevelWidth);
        widgets.Add(widthInput);

        heightInput = new NumberTextInput(new Rectangle(800, 0, 200, 50),
            currentLevel.Height,
            5,
            standardFont,
            _changeLevelHeight);
        widgets.Add(heightInput);
    }

    private List<string> _getAllLevelsNames()
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

    private void _changeLevel(int levelIndex)
    {
        levelName = levelNames[levelIndex];
        if (levelIndex == levelNames.Count - 1)
            _createNewLevel();
        else
            _changeExistingLevel(levelNames[levelIndex]);
    }

    private void _createNewLevel()
    {
        currentLevel = new Level(new List<GameObject>());
        camera.SetNewMapSize(new System.Drawing.Size(currentLevel.Width, currentLevel.Height));
        ChangeWidthInputValue(currentLevel.Width);
        ChangeHeightInputValue(currentLevel.Height);
    }

    private void _changeExistingLevel(string levelName)
    {
        currentLevel = Level.CreateLevelFromFile(levelName);
        camera.SetNewMapSize(new System.Drawing.Size(currentLevel.Width, currentLevel.Height));
        ChangeWidthInputValue(currentLevel.Width);
        ChangeHeightInputValue(currentLevel.Height);
    }

    private Vector2 _getGridCellIndex(int x, int y)
    {
        return new Vector2(x / gridSize, y / gridSize);
    }

    private void _createModeAction(MouseState mouseState)
    {
        var gridCell = _getGridCellIndex((int)(mouseState.X + cameraX - (Resolution.EtalonWidth / 2)), (int)(mouseState.Y + cameraY - (Resolution.EtalonHeight / 2)));
        //if (FindCollisionObject(gridCell) != null)
        //    return;
        var coordinates = new Vector2(gridCell.X * gridSize, gridCell.Y * gridSize);
        var coordinatesInStrings = new string[] { coordinates.X.ToString(), coordinates.Y.ToString() };
        var objectType = TypesUtils.GetTypeFromString(currentGameObject.ToString());

        if (currentGameObject == LevelObjects.EarthBlock)
        {
            if (earthBlockCoordinates.X == float.NegativeInfinity && earthBlockCoordinates.Y == float.NegativeInfinity)
                earthBlockCoordinates = new Vector2(coordinates.X, coordinates.Y); 
            else
            {
                var earthBlockGridCell = _getGridCellIndex((int)earthBlockCoordinates.X, (int)earthBlockCoordinates.Y);
                if (gridCell.X >= earthBlockGridCell.X && gridCell.Y >= earthBlockGridCell.Y)
                {
                    var width = (gridCell.X - earthBlockGridCell.X) * gridSize + gridSize;
                    var height = (gridCell.Y - earthBlockGridCell.Y) * gridSize + gridSize;

                    var blockCoordinates = new[] { 
                        earthBlockCoordinates.X.ToString(),
                        earthBlockCoordinates.Y.ToString(),
                        width.ToString(),
                        height.ToString() 
                    };

                    var newGameObject = (GameObject)TypesUtils.CreateObject(typeof(EarthBlock), blockCoordinates);
                    currentLevel.AddToGameObjects(newGameObject);
                }

                earthBlockCoordinates = new Vector2(float.NegativeInfinity, float.NegativeInfinity);   
            }
        }
        else
        {
            if (currentGameObject == LevelObjects.Player && _checkIfPlayerAlreadyExists())
                return;
            var newGameObject = (GameObject)TypesUtils.CreateObject(objectType, coordinatesInStrings);
            currentLevel.AddToGameObjects(newGameObject);
        }
    }

    private void _deleteModeAction(MouseState mouseState)
    {
        var gridCell = _getGridCellIndex((int)(mouseState.X + cameraX - (Resolution.EtalonWidth / 2)), (int)(mouseState.Y + cameraY - (Resolution.EtalonHeight / 2)));
        var collisionObject = _findCollisionObject(gridCell);
        if (collisionObject != null)
            currentLevel.GameObjects.Remove(collisionObject);
    }

    private bool _checkIfPlayerAlreadyExists()
    {
        foreach (var gameObject in currentLevel.GameObjects)
            if (gameObject is Player)
                return true;
        return false;
    }

    private GameObject _findCollisionObject(Vector2 gridCell)
    {
        foreach (var gameObject in currentLevel.GameObjects)
            if (gridCell.X * gridSize >= gameObject.X && gridCell.X * gridSize <= gameObject.X + gameObject.Width
                && gridCell.Y * gridSize >= gameObject.Y && gridCell.Y * gridSize <= gameObject.Y + gameObject.Height)
                return gameObject;
        return null;
    }

    private void _changeLevelWidth(int width)
    {
        if (width >= Resolution.EtalonWidth)
        {
            currentLevel.Width = width;
            camera.SetNewMapSize(new System.Drawing.Size(width, currentLevel.Height));
            ChangeWidthInputValue(width);
        }
    }

    private void _changeLevelHeight(int height)
    {
        if (height >= Resolution.EtalonHeight)
        {
            currentLevel.Height = height;
            camera.SetNewMapSize(new System.Drawing.Size(currentLevel.Width, height));
            ChangeHeightInputValue(height);
        }
    }

    private void ChangeWidthInputValue(int value)
    {
        widthInput.SetValue(value);
    }

    private void ChangeHeightInputValue(int value)
    {
        heightInput.SetValue(value);
    }
}