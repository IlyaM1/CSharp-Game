using Dyhar.src.Control;
using Dyhar.src.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dyhar.src.UI;

public abstract class Widget
{
    public abstract void Update(Camera camera, InputManager control, MouseState mouseState, KeyboardState keyboardState);
    public abstract void Draw(SpriteBatch spriteBatch);
}
