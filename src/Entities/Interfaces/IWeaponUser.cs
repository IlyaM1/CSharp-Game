using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Mechanics;
using Microsoft.Xna.Framework;

namespace Dyhar.src.Entities.Interfaces;

public interface IWeaponUser
{
    public Vector2 FindWeaponStart();
    public void onAttack();
    public bool IsAttacking();
    public MeleeWeapon GetCurrentWeapon();
    public Direction GetDirection();
    public Reload GetReload();
}
