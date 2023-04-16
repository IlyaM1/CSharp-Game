using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dyhar.src.Entities;
using Dyhar.src.Control;
using Dyhar.src.LevelsCreator;
using System.Linq;

namespace Dyhar
{
    public class Dyhar : Game
    {
        public static readonly int width = 1920;
        public static readonly int height = 1080;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont standardFont;

        Player player;
        Control control = new Control(ControlState.Game);

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
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();

            player = new Player(150, 0);
            control.SetPlayer(player);

            currentLevel = new Level(new[] {(GameObject)player}.ToList());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.sprite = Content.Load<Texture2D>("player");
            standardFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var currentMouseState = Mouse.GetState();
            var currentKeyboardState = Keyboard.GetState();
            control.onUpdate(currentMouseState, currentKeyboardState);

            for (var i = 0; i < currentLevel.gameObjects.Count; i++)
            {
                currentLevel.physic.Move(currentLevel.gameObjects[0], currentLevel);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(Player.sprite, player.Position, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}