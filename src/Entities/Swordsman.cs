using Dyhar.src.Entities.AbstractClasses;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Mechanics;
using Dyhar.src.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dyhar.src.Entities;

public class Swordsman : Enemy, IWeaponUser
{
    public Swordsman(int x, int y) : base(x, y)
    {
        _sword = new Sword(this);
        Speed = 9;
        _sword.AttackDuration = 1000;
        _attackAnimationReload = new Reload(_sword.AttackDuration);
    }

    public override void PlayerEnteredScreenEventHandler(Player player)
    {
        if (BotStates.Wander == CurrentState && (Y >= player.Y - 40 && Y <= player.Y + 40))
            UpdateState(BotStates.Chase);

        if (CurrentState == BotStates.Chase)
        {
            var distanceToPlayer = (player.Position - Position).Length();
            if (player.X <= X)
                _direction = Direction.Left;
            else if (player.X > X)
                _direction = Direction.Right;

            if (player.Y < Y)
                Jump();

            if (distanceToPlayer < _sword.WeaponLength - 20)
                UpdateState(BotStates.Attack);
        }
    }

    public override void UpdateState(BotStates nextState)
    {
        CurrentState = nextState;
        switch (nextState)
        {
            case BotStates.Wander:
                Speed = 3;
                break;
            case BotStates.Chase:
                Speed = 10;
                break;
            case BotStates.Attack:
                Speed = 0;
                break;
        }
    }

    private void ExecuteState()
    {
        switch (CurrentState)
        {
            default:
                break;
            case BotStates.Idle:
                break;
            case BotStates.Wander:
                if (X == 0)
                    _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
                MoveHorizontally(_direction);
                break;
            case BotStates.Chase:
                MoveHorizontally(_direction);
                break;
            case BotStates.Attack:
                AttackingEventHandler();
                UpdateState(BotStates.Chase);
                break;
        }
    }

    public override void CollisionedEventHandler(GameObject collisionObject)
    {
        if (CurrentState == BotStates.Wander && TypesUtils.CanBeDownCasted<GameObject, EarthBlock>(collisionObject))
        {
            if (collisionObject.X + collisionObject.Width <= X)
                _direction = Direction.Right;
            else if (collisionObject.X >= X+Width)
                _direction = Direction.Left;
        }
    }

    public override void MoveHorizontally(Direction direction)
    {
        _direction = direction;
        base.MoveHorizontally(direction);
    }

    public void Jump()
    {
        if (!_isInJump)
        {
            _isInJump = true;
            Force.Y = -_jumpPower;
            return;
        }
    }

    public override void UpdatingEventHandler(GameTime gameTime)
    {
        _checkAllReloads(gameTime);
        ExecuteState();
    }

    public void AttackingEventHandler()
    {
        if (_attackAnimationReload.State == ReloadState.NotStarted 
            && _delayBetweenAttacks.State == ReloadState.NotStarted)
        {
            _sword.AttackingEventHandler();
            _attackAnimationReload.Start();
            _delayBetweenAttacks.Start();
        } 
    }

    public override void FalledOnGroundEventHandler()
    {
        _isInJump = false;
    }

    public static Texture2D Sprite;

    public override Texture2D GetSprite() => Sprite;
    public override double GetCurrentHp() => CurrentHealthPoints;
    public Vector2 GetWeaponStartPosition() => new Vector2(X + 20, Y + 20);
    public MeleeWeapon GetCurrentWeapon() => _sword;
    public Direction GetDirection() => _direction;
    public Reload GetAnimationReload() => _attackAnimationReload;


    private Direction _direction = new Random().Next(10) >= 5 ? Direction.Left : Direction.Right;
    private MeleeWeapon _sword;
    private Reload _attackAnimationReload;

    private bool _isInJump = false;
    private int _jumpPower = 18;

    private void _checkAllReloads(GameTime gameTime)
    {
        _attackAnimationReload.UpdatingEventHandler(gameTime);
        _delayBetweenAttacks.UpdatingEventHandler(gameTime);
    }

    private Reload _delayBetweenAttacks = new Reload(1000);
}