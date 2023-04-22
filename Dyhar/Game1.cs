﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Dyhar.src.Entities;
using Dyhar.src.Control;
using Dyhar.src.LevelsCreator;
using Dyhar.src.Utils;

using System.Linq;

namespace Dyhar
{
    public class Dyhar : Game
    {
        public static readonly int width = 1300;
        public static readonly int height = 800;

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

            LoadContent();

            player = new Player(1000, 0);
            control.SetPlayer(player);

            currentLevel = new Level(new[] {(GameObject)player}.ToList());

            for (var i = 10; i < 20;i++)
                currentLevel.gameObjects.Add(new EarthBlock(i * 25, 500));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.sprite = Content.Load<Texture2D>("player");
            standardFont = Content.Load<SpriteFont>("galleryFont");
            EarthBlock.sprite = Content.Load<Texture2D>("Earth2");
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

            _spriteBatch.Begin();
            for (var i = 0; i < currentLevel.gameObjects.Count; i++)
            {
                var gameObject = currentLevel.gameObjects[i];
                _spriteBatch.Draw(gameObject.GetSprite(), gameObject.Position, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}