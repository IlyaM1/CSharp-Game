using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;

namespace Dyhar.src.Entities.Interfaces;

public interface IWeaponUser
{
    public Vector2 FindWeaponStart();
    public MeleeWeapon GetCurrentWeapon();
    public Direction GetDirection();
    public Reload GetAnimationReload();

    public void onAttack();
    public void onHitOtherWarrior(GameObject gameObject) { }

    public virtual bool IsAttacking() => GetAnimationReload().State == ReloadState.Reloading;
}
