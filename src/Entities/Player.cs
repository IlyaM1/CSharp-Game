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
        public static Texture2D sprite;

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 16.0f;
            Force = new Vector2(0, 0);
            IsSolid = false;
            directionLook = Direction.Right;
            currentWeapon = new Sword(this);
            attackAnimationReload = new Reload(currentWeapon.AttackDuration);
            multipleJumpsReload = new Reload(10, onMultipleJumpsReloadFinish);
            HealthPoints = maxHealthPoints;
        }

        public void Dash(Camera camera)
        {
            if (camera is null)
                throw new ArgumentNullException("Camera in control not setted");

            if (canDash)
            {
                Force.X += directionLook == Direction.Right ? dashPower : -dashPower;

                dashAnimationReload.Start();
                dashReload.Start();
            } 
        }

        public void MoveHorizontally(Direction direction)
        {
            directionLook = direction;

            if (direction == Direction.Left)
                Force.X -= Speed;
            else if (direction == Direction.Right)
                Force.X += Speed;
            else
                throw new ArgumentException("Impossible Direction");
        }

        public void Jump()
        {
            if (!IsInJump)
            {
                IsInJump = true;
                Force.Y = -JumpPower;
                return;
            }
            else if (IsInJump && numberOfExtraJumps > 0)
            {
                numberOfExtraJumps -= 1;
                multipleJumpsReload.Start();
                Force.Y = -JumpPower;
                return;
            }
        }

        public override void onIsOnGround()
        {
            IsInJump = false;
        }

        public override void onUpdate(GameTime gameTime)
        {
            CheckAllReloads(gameTime);
        }

        public Vector2 FindWeaponStart()
        {
            if (directionLook == Direction.Right)
                return new Vector2((float)(X + Size.Width), (float)(Y + (Size.Height / 2)));
            if (directionLook == Direction.Left)
                return new Vector2((float)(X), (float)(Y + (Size.Height / 2)));

            throw new Exception("Impossible Direction");
        }

        public void onAttack()
        {
            currentWeapon.onAttack();
            attackAnimationReload.Start();
        }

        public override void onAttacked(MeleeWeapon weapon)
        {
            if (lastAttackNumber == weapon.CurrentAttackNumber)
                return;
            else
                lastAttackNumber = weapon.CurrentAttackNumber;

            HealthPoints -= weapon.Damage;
            if (HealthPoints <= 0)
                onDeath();
        }

        public void onHitOtherWarrior(GameObject gameObject)
        {
            if (!gameObject.IsAlive)
                HealthPoints += vampirismPower;
        }

        public override Texture2D GetSprite() => sprite;
        public MeleeWeapon GetCurrentWeapon() => currentWeapon;
        public Direction GetDirection() => directionLook;
        public Reload GetAnimationReload() => attackAnimationReload;
        public double GetCurrentHp() => HealthPoints;
        public Vector2 GetPosition() => Position;
        public Vector2 GetSize() => new Vector2(Size.Width, Size.Height);
        public double GetMaxHp() => MaxHealthPoints;

        public double MaxHealthPoints
        {
            get => maxHealthPoints;
            private set
            {
                if (value < 1)
                    maxHealthPoints = 1;
                else
                    maxHealthPoints = value;
            }
        }

        public double HealthPoints
        {
            get => currentHealthPoints;
            private set
            {
                if (value < 0)
                    currentHealthPoints = 0;
                else if (value > maxHealthPoints)
                    currentHealthPoints = maxHealthPoints;
                else
                    currentHealthPoints = value;
            }
        }


        bool IsInJump = false;
        int numberOfExtraJumps = 1;
        int maxNumberOfExtraJumps = 1;
        int JumpPower = 15;

        Reload multipleJumpsReload;
        double maxHealthPoints = 200;
        double currentHealthPoints = 200;

        Direction directionLook;
        MeleeWeapon currentWeapon;
        Reload attackAnimationReload;

        private ulong lastAttackNumber = 0;

        private int dashPower = 800;
        private bool canDash => dashReload.State == ReloadState.NotStarted;
        Reload dashAnimationReload = new Reload(50);
        Reload dashReload = new Reload(2000);

        int vampirismPower = 30; // How much we heal after defeating enemy

        void CheckAllReloads(GameTime gameTime)
        {
            multipleJumpsReload.OnUpdate(gameTime);
            attackAnimationReload.OnUpdate(gameTime);
            dashAnimationReload.OnUpdate(gameTime);
            dashReload.OnUpdate(gameTime);
        }

        void onMultipleJumpsReloadFinish()
        {
            if (numberOfExtraJumps < maxNumberOfExtraJumps)
            {
                numberOfExtraJumps += 1;
                if (numberOfExtraJumps < maxNumberOfExtraJumps)
                    multipleJumpsReload.Start();
            }
        }
    }
}