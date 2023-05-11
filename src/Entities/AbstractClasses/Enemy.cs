using Dyhar.src.Entities.Interfaces;
namespace Dyhar.src.Entities.AbstractClasses;

public abstract class Enemy : MovingGameObject, IWarrior
{
    public abstract double GetCurrentHp();
    public abstract void onPlayerScreen(Player player);
}
