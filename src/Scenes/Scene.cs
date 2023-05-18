using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace Dyhar.src.Scenes;

public abstract class Scene
{
    protected KeyboardState prevKeyboardState;

    public bool IsDone { get; protected set; }
    public Type SceneToRun { get; protected set; }

    public abstract void LoadContent(ContentManager content, GraphicsDevice graphics);
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}