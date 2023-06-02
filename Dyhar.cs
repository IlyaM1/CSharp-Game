using Dyhar.src.Drawing;
using Dyhar.src.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar;

public class Dyhar : Game
{
    public Scene CurrentScene { get; private set; }

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
        CurrentScene.LoadContent(Content, GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (CurrentScene.IsDone)
            if (CurrentScene.SceneToRun != null)
                _runScene((Scene)Activator.CreateInstance(CurrentScene.SceneToRun));
            else
                Exit();
        else
            CurrentScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        CurrentScene.Draw(_spriteBatch);
        base.Draw(gameTime);
    }

    private static Dyhar _dyharInstance;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Dyhar()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        CurrentScene = new MenuScene();

        _graphics = new GraphicsDeviceManager(this);
    }

    private void _runScene(Scene scene)
    {
        CurrentScene = scene;
        scene.LoadContent(Content, GraphicsDevice);
    }
}