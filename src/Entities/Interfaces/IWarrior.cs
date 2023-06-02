using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Dyhar.src.Drawing;

namespace Dyhar.src.Entities.Interfaces;

public interface IWarrior
{
    public Vector2 GetPosition();
    public Vector2 GetSize();
    public double GetCurrentHp();
    public double GetMaxHp();

    public virtual void DrawHealthBar(SpriteBatch spriteBatch)
    {
        var position = GetPosition();
        var size = GetSize();
        var currentHp = GetCurrentHp();
        var maxHp = GetMaxHp();

        var leftPoint = new Vector2(position.X - 20, position.Y - 40);
        var healthBarSize = new Vector2(size.X + 40, 20);
        var greenLineSize = new Vector2((float)((currentHp / maxHp) * healthBarSize.X)-2, 18);

        spriteBatch.Draw(HealthBarSprites.EmptyHpBarSprite,
            new Rectangle((int)leftPoint.X, (int)leftPoint.Y, (int)healthBarSize.X, (int)healthBarSize.Y),
            Color.White);

        spriteBatch.Draw(HealthBarSprites.GreenColorSprite,
            new Rectangle((int)leftPoint.X + 1, (int)leftPoint.Y + 1, (int)greenLineSize.X, (int)greenLineSize.Y),
            Color.White);
    }
}
