using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Dyhar.src.Entities;
using Dyhar.src.Control;
using Dyhar.src.LevelsCreator;
using Dyhar.src.Utils;
using Dyhar.src.Drawing;
using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;

namespace Dyhar
{
    public class Dyhar : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont standardFont;

        Player player;
        Control control = new Control(ControlState.Game);
        Camera camera;

        Level currentLevel;

        Texture2D _gridCube;

        public Dyhar()
        {
            _graphics = new GraphicsDeviceManager(this);
            

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            currentLevel = Level.CreateTestLevel(control, out player);

            //this.IsFixedTimeStep = true;//false;
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);
        }

        protected override void Initialize()
        {
            // _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = Resolution.actualWidth;
            _graphics.PreferredBackBufferHeight = Resolution.actualHeight;
            _graphics.ApplyChanges();
            LoadContent();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.sprite = Content.Load<Texture2D>("player");
            EarthBlock.sprite = Content.Load<Texture2D>("Earth2");
            Sword.sprite = Content.Load<Texture2D>("Sword");
            Swordsman.sprite = Content.Load<Texture2D>("Swordsman");
            _gridCube = Content.Load<Texture2D>("GridCube1");

            standardFont = Content.Load<SpriteFont>("galleryFont");

            camera = new Camera(_graphics.GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            control.onUpdate(Mouse.GetState(), Keyboard.GetState());

            for (var i = 0; i < currentLevel.GameObjects.Count; i++)
            {
                var gameObject = currentLevel.GameObjects[i];
                if (TypesUtils.CanBeDownCasted<GameObject, MovingGameObject>(gameObject))
                    currentLevel.Physics.Move((MovingGameObject)gameObject);

                foreach (var weaponUser in currentLevel.GetWeaponUsers())
                    if (weaponUser.IsAttacking())
                        if (weaponUser.GetCurrentWeapon().CheckCollision(gameObject, weaponUser.GetDirection()))
                            gameObject.onAttacked(weaponUser.GetCurrentWeapon());

                if (!gameObject.IsAlive)
                    currentLevel.GameObjects.Remove(gameObject);

                gameObject.onUpdate(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.Update(player.Position, currentLevel.Width, currentLevel.Height);
            _spriteBatch.Begin(transformMatrix: camera.Transform);
            for (var i = 0; i < currentLevel.GameObjects.Count; i++)
            {
                var gameObject = currentLevel.GameObjects[i];
                gameObject.Draw(_spriteBatch);

                if (TypesUtils.CanBeDownCasted<GameObject, IWeaponUser>(gameObject))
                {
                    var weaponUser = (IWeaponUser)gameObject;
                    if (weaponUser.IsAttacking())
                        weaponUser.GetCurrentWeapon().Draw(_spriteBatch);
                }
            }

            //for (var i = 0; i < currentLevel.Width; i += 50)
            //    for (var j = 0; j < currentLevel.Height; j+=50)
            //        _spriteBatch.Draw(_gridCube, new Rectangle(i, j, 50, 50), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}