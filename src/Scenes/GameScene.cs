using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Entities;
using Dyhar.src.Utils;
using Dyhar.src.Levels;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Dyhar.src.Scenes;

public class GameScene : Scene
{
    public GameScene()
    {
        _currentLevel = Level.CreateLevelFromFile("Level1", _control, out _player);

        //this.IsFixedTimeStep = true;//false;
        //this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);
    }

    public override void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        Player.Sprite = content.Load<Texture2D>("player");
        EarthBlock.Sprite = content.Load<Texture2D>("Earth2");
        Sword.Sprite = content.Load<Texture2D>("Sword");
        Swordsman.Sprite = content.Load<Texture2D>("Swordsman");

        HealthBarSprites.EmptyHpBarSprite = content.Load<Texture2D>("EmptyHpBar");
        HealthBarSprites.GreenColorSprite = content.Load<Texture2D>("GreenCube");

        _standardFont = content.Load<SpriteFont>("galleryFont");

        _camera = new Camera(graphics.Viewport, _currentLevel.Width, _currentLevel.Height);
        _control.SetCamera(_camera);
    }

    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            IsDone = true;
            SceneToRun = typeof(MenuScene);
        }

        _control.Update(Mouse.GetState(), Keyboard.GetState());

        for (var i = 0; i < _currentLevel.GameObjects.Count; i++)
        {
            var gameObject = _currentLevel.GameObjects[i];
            if (TypesUtils.CanBeDownCasted<GameObject, MovingGameObject>(gameObject))
                _currentLevel.Physics.HandleObjectMovementAndCollisions((MovingGameObject)gameObject);

            if (TypesUtils.CanBeDownCasted<GameObject, Enemy>(gameObject))
            {
                var enemy = (Enemy)gameObject;
                if (enemy.IsOnPlayerScreen(_camera))
                    enemy.PlayerEnteredScreenEventHandler(_player);
            }

            foreach (var weaponUser in _currentLevel.GetWeaponUsers())
                if (weaponUser.IsAttacking())
                    if (weaponUser.GetCurrentWeapon().CheckCollision(gameObject, weaponUser.GetDirection()))
                    {
                        gameObject.GotAttackedEventHandler(weaponUser.GetCurrentWeapon());
                        weaponUser.HittedOtherWarriorEventHandler(gameObject);
                    }
                        

            if (!gameObject.IsAlive)
                _currentLevel.GameObjects.Remove(gameObject);

            gameObject.UpdatingEventHandler(gameTime);
        }

        if (_checkIfLevelEnded())
            _moveOnNextLevel();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _camera.Update(_player.Position);
        spriteBatch.Begin(transformMatrix: _camera.Transform);

        for (var i = 0; i < _currentLevel.GameObjects.Count; i++)
        {
            var gameObject = _currentLevel.GameObjects[i];
            gameObject.Draw(spriteBatch);

            if (TypesUtils.CanBeDownCasted<GameObject, IWeaponUser>(gameObject))
            {
                var weaponUser = (IWeaponUser)gameObject;
                if (weaponUser.IsAttacking())
                    weaponUser.GetCurrentWeapon().Draw(spriteBatch);
            }

            if (TypesUtils.CanBeDownCasted<GameObject, IWarrior>(gameObject))
                ((IWarrior)gameObject).DrawHealthBar(spriteBatch);
        }

        spriteBatch.End();
    }


    private SpriteFont _standardFont;

    private Player _player;
    private GameControl _control = new GameControl();
    private Camera _camera;

    private int _levelNumber = 1;
    private Level _currentLevel;

    private bool _checkIfLevelEnded()
    {
        if (_player.X + 200 >= _currentLevel.Width)
            if (_currentLevel.EnemyCount == 0)
                return true;
        return false;
    }

    private void _moveOnNextLevel()
    {
        _levelNumber += 1;
        _currentLevel = Level.CreateLevelFromFile("Level" + _levelNumber.ToString(), _control, out _player);
        _camera.SetNewMapSize(new System.Drawing.Size(_currentLevel.Width, _currentLevel.Height));
    }
}