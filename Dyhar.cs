using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Dyhar.src.Scenes;
using Dyhar.src.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar
{
    public class Dyhar : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;
        Scene currentScene;

        GameControl control;

        public Dyhar()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics = new GraphicsDeviceManager(this);
            currentScene = new MenuScene(); // Запуск сцены меню

            control = new GameControl();
        }

        protected override void Initialize()
        {
            // _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = Resolution.actualWidth;
            _graphics.PreferredBackBufferHeight = Resolution.actualHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentScene.LoadContent(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (currentScene.IsDone)
                if (currentScene.SceneToRun != null)
                    RunScene((Scene)Activator.CreateInstance(currentScene.SceneToRun));
                else
                    Exit();
            else
                currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // spriteBatch.Begin();
            currentScene.Draw(spriteBatch);
            // spriteBatch.End();
            base.Draw(gameTime);
        }

        private void RunScene(Scene scene)
        {
            currentScene = scene;
            scene.LoadContent(Content, GraphicsDevice);
        }
    }
}