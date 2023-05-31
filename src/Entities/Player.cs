using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dyhar.src.Mechanics;
using System;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Entities.AbstractClasses;
using Microsoft.Xna.Framework.Input;
using Dyhar.src.Drawing;

namespace Dyhar.src.Entities
{
    public class Player : MovingGameObject, IWarrior, IWeaponUser
    {
        public static Texture2D Sprite;

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 12.0f;
            Force = new Vector2(0, 0);
            IsSolid = false;
            _directionLook = Direction.Right;
            _currentWeapon = new Sword(this);
            _attackAnimationReload = new Reload(_currentWeapon.AttackDuration);
            _multipleJumpsReload = new Reload(1000, _multipleJumpsReloadedEventHandler);
            HealthPoints = _maxHealthPoints;
        }

        public void Dash(Camera camera)
        {
            if (camera is null)
                throw new ArgumentNullException("Camera in control not setted");

            if (_canDash)
            {
                Force.X += _directionLook == Direction.Right ? _dashPower : -_dashPower;

                _dashAnimationReload.Start();
                _dashReload.Start();
            } 
        }

        public override void MoveHorizontally(Direction direction)
        {
            _directionLook = direction;
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
            else if (_isInJump && _numberOfExtraJumps > 0)
            {
                _numberOfExtraJumps -= 1;
                _multipleJumpsReload.Start();
                Force.Y = -_jumpPower;
                return;
            }
        }

        public override void FalledOnGroundEventHandler()
        {
            _isInJump = false;
        }

        public override void UpdatingEventHandler(GameTime gameTime)
        {
            _checkAllReloads(gameTime);
        }

        public Vector2 GetWeaponStartPosition()
        {
            if (_directionLook == Direction.Right)
                return new Vector2((float)(X + Size.Width), (float)(Y + (Size.Height / 2)));
            if (_directionLook == Direction.Left)
                return new Vector2((float)(X), (float)(Y + (Size.Height / 2)));

            throw new Exception("Impossible Direction");
        }

        public void AttackingEventHandler()
        {
            _currentWeapon.AttackingEventHandler();
            _attackAnimationReload.Start();
        }

        public override void GotAttackedEventHandler(MeleeWeapon weapon)
        {
            if (_lastAttackNumber == weapon.CurrentAttackNumber)
                return;
            else
                _lastAttackNumber = weapon.CurrentAttackNumber;

            HealthPoints -= weapon.Damage;
            if (HealthPoints <= 0)
                DyingEventHandler();
        }

        public void HittedOtherWarriorEventHandler(GameObject gameObject)
        {
            if (!gameObject.IsAlive)
                HealthPoints += _vampirismPower;
        }

        public override Texture2D GetSprite() => Sprite;
        public MeleeWeapon GetCurrentWeapon() => _currentWeapon;
        public Direction GetDirection() => _directionLook;
        public Reload GetAnimationReload() => _attackAnimationReload;
        public double GetCurrentHp() => HealthPoints;
        public Vector2 GetPosition() => Position;
        public Vector2 GetSize() => new Vector2(Size.Width, Size.Height);
        public double GetMaxHp() => MaxHealthPoints;

        public double MaxHealthPoints
        {
            get => _maxHealthPoints;
            private set
            {
                if (value < 1)
                    _maxHealthPoints = 1;
                else
                    _maxHealthPoints = value;
            }
        }

        public double HealthPoints
        {
            get => _currentHealthPoints;
            private set
            {
                if (value < 0)
                    _currentHealthPoints = 0;
                else if (value > _maxHealthPoints)
                    _currentHealthPoints = _maxHealthPoints;
                else
                    _currentHealthPoints = value;
            }
        }


        private bool _isInJump = false;
        private int _numberOfExtraJumps = 1;
        private int _maxNumberOfExtraJumps = 1;
        private int _jumpPower = 15;

        private Reload _multipleJumpsReload;
        private double _maxHealthPoints = 200;
        private double _currentHealthPoints = 200;

        private Direction _directionLook;
        private MeleeWeapon _currentWeapon;
        private Reload _attackAnimationReload;

        private ulong _lastAttackNumber = 0;

        private int _dashPower = 300;
        private bool _canDash => _dashReload.State == ReloadState.NotStarted;
        private Reload _dashAnimationReload = new Reload(50);
        private Reload _dashReload = new Reload(700);

        private int _vampirismPower = 30; // How much we heal after defeating enemy

        private void _checkAllReloads(GameTime gameTime)
        {
            _multipleJumpsReload.UpdatingEventHandler(gameTime);
            _attackAnimationReload.UpdatingEventHandler(gameTime);
            _dashAnimationReload.UpdatingEventHandler(gameTime);
            _dashReload.UpdatingEventHandler(gameTime);
        }

        private void _multipleJumpsReloadedEventHandler()
        {
            if (_numberOfExtraJumps < _maxNumberOfExtraJumps)
            {
                _numberOfExtraJumps += 1;
                if (_numberOfExtraJumps < _maxNumberOfExtraJumps)
                    _multipleJumpsReload.Start();
            }
        }
    }
}