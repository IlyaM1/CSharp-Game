using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;

namespace Dyhar.src.Entities.Interfaces;

public interface IWeaponUser
{
    public Vector2 GetWeaponStartPosition();
    public MeleeWeapon GetCurrentWeapon();
    public Direction GetDirection();
    public Reload GetAnimationReload();

    public void AttackingEventHandler();
    public void HittedOtherWarriorEventHandler(GameObject gameObject) { }

    public virtual bool IsAttacking() => GetAnimationReload().State == ReloadState.Reloading;
}
