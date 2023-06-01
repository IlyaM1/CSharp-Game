using Dyhar.src.Drawing;
using Dyhar.src.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar;

public class Dyhar : Game
{
    public static Dyhar GetInstance()
    {
        if (_dyharInstance == null)
            _dyharInstance = new Dyhar();

        return _dyharInstance;
    }

    protected override void Initialize()
    {
        // _graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = Resolution.ActualWidth;
        _graphics.PreferredBackBufferHeight = Resolution.ActualHeight;
        _graphics.ApplyChanges();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _currentScene.LoadContent(Content, GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_currentScene.IsDone)
            if (_currentScene.SceneToRun != null)
                _runScene((Scene)Activator.CreateInstance(_currentScene.SceneToRun));
            else
                Exit();
        else
            _currentScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _currentScene.Draw(_spriteBatch);
        base.Draw(gameTime);
    }

    private static Dyhar _dyharInstance;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Scene _currentScene;

    private Dyhar()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics = new GraphicsDeviceManager(this);
        _currentScene = new MenuScene();
    }

    private void _runScene(Scene scene)
    {
        _currentScene = scene;
        scene.LoadContent(Content, GraphicsDevice);
    }
}