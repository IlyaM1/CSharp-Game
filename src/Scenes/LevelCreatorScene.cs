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
        _currentLevel = Level.CreateLevelFromFile(_levelName);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: _camera.Transform);

        _drawLevel(spriteBatch);

        foreach (var widget in _widgets)
            widget.Draw(spriteBatch);

        spriteBatch.End();
        _camera.Update(_cameraPosition);
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        Player.Sprite = content.Load<Texture2D>("player");
        EarthBlock.Sprite = content.Load<Texture2D>("Earth2");
        Sword.Sprite = content.Load<Texture2D>("Sword");
        Swordsman.Sprite = content.Load<Texture2D>("Swordsman");

        _gridCube = content.Load<Texture2D>("GridCube1");

        var standardFont = content.Load<SpriteFont>("galleryFont");

        DropdownList.Font = standardFont;
        DropdownList.ElementsSprite = content.Load<Texture2D>("DropdownWidgetElement");
        DropdownList.BackgroundSprite = content.Load<Texture2D>("DropdownWidget");

        NumberTextInput.Font = standardFont;
        Label.Font = standardFont;

        _camera = new Camera(graphics.Viewport, _currentLevel.Width, _currentLevel.Height);

        _createAllWidgets();
    }

    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            RunNewScene(typeof(MenuScene));

            var levelString = LevelDeserialization.DeserializeLevel(_currentLevel);
            if (_levelName == "New level")
                _levelName = "Level" + (levelNames.Count()).ToString();
            if (_checkIfPlayerAlreadyExists() && _currentLevel.EnemyCount > 0)
                LevelDeserialization.WriteToFile(_levelName, levelString.ToString());
        }

        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();

        foreach (var widget in _widgets)
            widget.UpdateEventHandler(_camera, _control, mouseState, keyboardState);

        _moveCameraUpdate(keyboardState);

        if (_control.IsMouseLeftDown(Mouse.GetState()))
        {
            if (currentMode == Mode.Creating)
                _control.PressLeftMouse(() => _createModeAction(mouseState));
            if (currentMode == Mode.Deleting)
                _control.PressLeftMouse(() => _deleteModeAction(mouseState));
        }

        _control.RemoveUnpressedKeys(keyboardState);
        _control.UnpressLeftMouse(mouseState);
    }



    private Level _currentLevel;
    private string _levelName = "Level1";
    private List<string> levelNames;
    private LevelObjects currentGameObject = LevelObjects.Player;
    private Mode currentMode = Mode.Creating;

    private Camera _camera;
    private float _cameraX = Resolution.EtalonWidth / 2;
    private float _cameraY = Resolution.EtalonHeight / 2;
    private Vector2 _cameraPosition => new Vector2(_cameraX, _cameraY);

    private InputManager _control = new InputManager();

    private List<Widget> _widgets = new List<Widget>();
    private NumberTextInput _widthInput;
    private NumberTextInput _heightInput;

    private Texture2D _gridCube;
    private int _gridSize = 50;

    private Vector2 _earthBlockCoordinates = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

    private void _drawLevel(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < _currentLevel.GameObjects.Count; i++)
        {
            var gameObject = _currentLevel.GameObjects[i];
            gameObject.Draw(spriteBatch);

            if (gameObject is IWeaponUser weaponUser)
                if (weaponUser.IsAttacking())
                    weaponUser.GetCurrentWeapon().Draw(spriteBatch);
        }

        for (var i = 0; i < _currentLevel.Width; i += _gridSize)
            for (var j = 0; j < _currentLevel.Height; j += _gridSize)
                spriteBatch.Draw(_gridCube, new Rectangle(i, j, _gridSize, _gridSize), Color.White);

        if (_earthBlockCoordinates.X != float.NegativeInfinity && _earthBlockCoordinates.Y != float.NegativeInfinity)
        {
            var mouse = Mouse.GetState();
            var mouseInMapCoordinates = _camera.ConvertScreenPositionToMapPosition(new Vector2(mouse.X, mouse.Y));

            for (var i = _earthBlockCoordinates.X; i < mouseInMapCoordinates.X; i += _gridSize)
                for (var j = _earthBlockCoordinates.Y; j < mouseInMapCoordinates.Y; j += _gridSize)
                    spriteBatch.Draw(EarthBlock.Sprite, new Rectangle((int)i, (int)j, _gridSize, _gridSize), Color.Gray);
        }
    }

    private void _moveCameraUpdate(KeyboardState keyboardState)
    {
        if (_control.IsKeyHold(keyboardState, Keys.A))
            _control.PressButton(Keys.A, () => _cameraX -= 20);

        if (_control.IsKeyHold(keyboardState, Keys.D))
            _control.PressButton(Keys.D, () => _cameraX += 20);

        if (_control.IsKeyHold(keyboardState, Keys.W))
            _control.PressButton(Keys.W, () => _cameraY -= 20);

        if (_control.IsKeyHold(keyboardState, Keys.S))
            _control.PressButton(Keys.S, () => _cameraY += 20);

        _cameraX = MathHelper.Clamp(_cameraX, (Resolution.EtalonWidth / 2), _currentLevel.Width - (Resolution.EtalonWidth / 2));
        _cameraY = MathHelper.Clamp(_cameraY, (Resolution.EtalonHeight / 2), _currentLevel.Height - (Resolution.EtalonHeight / 2));
    }

    private void _createAllWidgets()
    {
        levelNames = _getAllLevelsNames();
        levelNames.Add("New level");

        var levelChoose = new DropdownList(
            new Rectangle(0, 0, 200, 50),
            levelNames,
            x => _changeLevel(x)
            );

        _widgets.Add(levelChoose);

        var allGameObjectsNames = typeof(LevelObjects).GetEnumNames();
        var levelObjectChoose = new DropdownList(
            new Rectangle(200, 0, 200, 50),
            allGameObjectsNames.ToList(),
            x => currentGameObject = (LevelObjects)Enum.Parse(typeof(LevelObjects), allGameObjectsNames[x])
            );

        _widgets.Add(levelObjectChoose);

        var allModesNames = typeof(Mode).GetEnumNames();
        var modeChoose = new DropdownList(
            new Rectangle(400, 0, 200, 50),
            allModesNames.ToList(),
            x => currentMode = (Mode)Enum.Parse(typeof(Mode), allModesNames[x])
            );

        _widgets.Add(modeChoose);

        _widthInput = new NumberTextInput(new Rectangle(600, 0, 200, 50),
            _currentLevel.Width,
            5,
            _changeLevelWidth);
        _widgets.Add(_widthInput);

        _heightInput = new NumberTextInput(new Rectangle(800, 0, 200, 50),
            _currentLevel.Height,
            5,
            _changeLevelHeight);
        _widgets.Add(_heightInput);
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
        _levelName = levelNames[levelIndex];
        if (levelIndex == levelNames.Count - 1)
            _createNewLevel();
        else
            _changeExistingLevel(levelNames[levelIndex]);
    }

    private void _createNewLevel()
    {
        _currentLevel = new Level(new List<GameObject>());
        _camera.SetNewMapSize(new System.Drawing.Size(_currentLevel.Width, _currentLevel.Height));
        ChangeWidthInputValue(_currentLevel.Width);
        ChangeHeightInputValue(_currentLevel.Height);
    }

    private void _changeExistingLevel(string levelName)
    {
        _currentLevel = Level.CreateLevelFromFile(levelName);
        _camera.SetNewMapSize(new System.Drawing.Size(_currentLevel.Width, _currentLevel.Height));
        ChangeWidthInputValue(_currentLevel.Width);
        ChangeHeightInputValue(_currentLevel.Height);
    }

    private Vector2 _getGridCellIndex(int x, int y)
    {
        return new Vector2(x / _gridSize, y / _gridSize);
    }

    private void _createModeAction(MouseState mouseState)
    {
        var gridCell = _getGridCellIndex((int)(mouseState.X + _cameraX - (Resolution.EtalonWidth / 2)), (int)(mouseState.Y + _cameraY - (Resolution.EtalonHeight / 2)));
        var coordinates = new Vector2(gridCell.X * _gridSize, gridCell.Y * _gridSize);
        var coordinatesInStrings = new string[] { coordinates.X.ToString(), coordinates.Y.ToString() };
        var objectType = TypesUtils.GetTypeFromString(currentGameObject.ToString());

        if (currentGameObject == LevelObjects.EarthBlock)
        {
            if (_earthBlockCoordinates.X == float.NegativeInfinity && _earthBlockCoordinates.Y == float.NegativeInfinity)
                _earthBlockCoordinates = new Vector2(coordinates.X, coordinates.Y); 
            else
            {
                var earthBlockGridCell = _getGridCellIndex((int)_earthBlockCoordinates.X, (int)_earthBlockCoordinates.Y);
                if (gridCell.X >= earthBlockGridCell.X && gridCell.Y >= earthBlockGridCell.Y)
                {
                    var width = (gridCell.X - earthBlockGridCell.X) * _gridSize + _gridSize;
                    var height = (gridCell.Y - earthBlockGridCell.Y) * _gridSize + _gridSize;

                    var blockCoordinates = new[] { 
                        _earthBlockCoordinates.X.ToString(),
                        _earthBlockCoordinates.Y.ToString(),
                        width.ToString(),
                        height.ToString() 
                    };

                    var newGameObject = (GameObject)TypesUtils.CreateObject(typeof(EarthBlock), blockCoordinates);
                    _currentLevel.AddToGameObjects(newGameObject);
                }

                _earthBlockCoordinates = new Vector2(float.NegativeInfinity, float.NegativeInfinity);   
            }
        }
        else
        {
            if (currentGameObject == LevelObjects.Player && _checkIfPlayerAlreadyExists())
                return;
            var newGameObject = (GameObject)TypesUtils.CreateObject(objectType, coordinatesInStrings);
            _currentLevel.AddToGameObjects(newGameObject);
        }
    }

    private void _deleteModeAction(MouseState mouseState)
    {
        var gridCell = _getGridCellIndex((int)(mouseState.X + _cameraX - (Resolution.EtalonWidth / 2)), (int)(mouseState.Y + _cameraY - (Resolution.EtalonHeight / 2)));
        var collisionObject = _findCollisionObject(gridCell);
        if (collisionObject != null)
            _currentLevel.GameObjects.Remove(collisionObject);
    }

    private bool _checkIfPlayerAlreadyExists()
    {
        foreach (var gameObject in _currentLevel.GameObjects)
            if (gameObject is Player)
                return true;
        return false;
    }

    private GameObject _findCollisionObject(Vector2 gridCell)
    {
        foreach (var gameObject in _currentLevel.GameObjects)
            if (gridCell.X * _gridSize >= gameObject.X && gridCell.X * _gridSize <= gameObject.X + gameObject.Width
                && gridCell.Y * _gridSize >= gameObject.Y && gridCell.Y * _gridSize <= gameObject.Y + gameObject.Height)
                return gameObject;
        return null;
    }

    private void _changeLevelWidth(int width)
    {
        if (width >= Resolution.EtalonWidth)
        {
            _currentLevel.Width = width;
            _camera.SetNewMapSize(new System.Drawing.Size(width, _currentLevel.Height));
            ChangeWidthInputValue(width);
        }
    }

    private void _changeLevelHeight(int height)
    {
        if (height >= Resolution.EtalonHeight)
        {
            _currentLevel.Height = height;
            _camera.SetNewMapSize(new System.Drawing.Size(_currentLevel.Width, height));
            ChangeHeightInputValue(height);
        }
    }

    private void ChangeWidthInputValue(int value)
    {
        _widthInput.SetValue(value);
    }

    private void ChangeHeightInputValue(int value)
    {
        _heightInput.SetValue(value);
    }
}