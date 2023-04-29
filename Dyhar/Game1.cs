using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Dyhar.src.Entities;
using Dyhar.src.Control;
using Dyhar.src.LevelsCreator;
using Dyhar.src.Utils;

using System.Linq;
using Dyhar.src.Drawing;

namespace Dyhar
{
    public class Dyhar : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private VirtualResolution _virtualResolution;
        SpriteFont standardFont;

        Player player;
        Control control = new Control(ControlState.Game);
        Camera camera;

        Level currentLevel;

        public Dyhar()
        {
            _graphics = new GraphicsDeviceManager(this);
            

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = VirtualResolution.actualWidth;
            _graphics.PreferredBackBufferHeight = VirtualResolution.actualHeight;
            _graphics.ApplyChanges();

            LoadContent();

            player = new Player(0, 0);
            control.SetPlayer(player);

            currentLevel = new Level(new[] {(GameObject)player}.ToList());

            for (var i = 10; i < 20; i++)
                currentLevel.gameObjects.Add(new EarthBlock(i * 25, 600));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _virtualResolution = new VirtualResolution(_spriteBatch);

            Player.sprite = Content.Load<Texture2D>("player");
            standardFont = Content.Load<SpriteFont>("galleryFont");
            EarthBlock.sprite = Content.Load<Texture2D>("Earth2");

            camera = new Camera(_graphics.GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            control.onUpdate(Mouse.GetState(), Keyboard.GetState());

            for (var i = 0; i < currentLevel.gameObjects.Count; i++)
            {
                var gameObject = currentLevel.gameObjects[i];
                if (TypesUtils.CanBeDownCasted<GameObject, MovingGameObject>(gameObject))
                {
                    currentLevel.physic.Move((MovingGameObject)gameObject);
                }    
                gameObject.onUpdate(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            camera.Update(player.Position, currentLevel.Width, currentLevel.Height);
            _spriteBatch.Begin(transformMatrix: camera.Transform);
            for (var i = 0; i < currentLevel.gameObjects.Count; i++)
            {
                var gameObject = currentLevel.gameObjects[i];
                _virtualResolution.Draw(gameObject);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}