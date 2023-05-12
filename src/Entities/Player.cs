using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dyhar.src.Mechanics;
using System;
using Dyhar.src.Entities.Interfaces;
using Dyhar.src.Entities.AbstractClasses;

namespace Dyhar.src.Entities
{
    public class Player : MovingGameObject, IWarrior, IWeaponUser
    {
        public static Texture2D sprite;
        public Direction DirectionLook { get; private set; }
        public MeleeWeapon CurrentWeapon { get; private set; }
        public Reload AttackAnimationReload { get; private set; }

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0f;
            Force = new Vector2(0, 0);
            IsSolid = false;
            DirectionLook = Direction.Right;
            CurrentWeapon = new Sword(this);
            AttackAnimationReload = new Reload("attack_animation", CurrentWeapon.AttackDuration);
            currentHealthPoints = maxHealthPoints;
        }

        public void MoveHorizontally(Direction direction)
        {
            DirectionLook = direction;

            if (direction == Direction.Left)
                Force.X -= (float)Speed;
            else if (direction == Direction.Right)
                Force.X += (float)Speed;
            else
                throw new ArgumentException();
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

        public override Texture2D GetSprite() => sprite;

        public override void onCollision(GameObject collisionObject)
        {
            return;
        }

        public Vector2 FindWeaponStart()
        {
            if (DirectionLook == Direction.Right)
                return new Vector2((float)(X + Size.Width), (float)(Y + (Size.Height / 2)));
            if (DirectionLook == Direction.Left)
                return new Vector2((float)(X), (float)(Y + (Size.Height / 2)));

            throw new Exception("Impossible Direction");
        }

        public void onAttack()
        {
            AttackAnimationReload.Start();
        }

        public bool IsAttacking() => AttackAnimationReload.State == ReloadState.Reloading;
        public MeleeWeapon GetCurrentWeapon() => CurrentWeapon;
        public Direction GetDirection() => DirectionLook;
        public Reload GetReload() => AttackAnimationReload;
        public double GetCurrentHp() => currentHealthPoints;




        bool IsInJump = false;
        int numberOfExtraJumps = 1;
        int maxNumberOfExtraJumps = 3;
        int JumpPower = 15;

        Reload multipleJumpsReload = new Reload("jump_reload", 10);
        double maxHealthPoints = 100.0;
        double currentHealthPoints = 100.0;

        private void CheckAllReloads(GameTime gameTime)
        {
            multipleJumpsReload.OnUpdate(gameTime);
            if (multipleJumpsReload.State == ReloadState.Finished)
            {
                if (numberOfExtraJumps < maxNumberOfExtraJumps)
                {
                    numberOfExtraJumps += 1;
                    multipleJumpsReload.CompletedFinishedCheck();
                    if (numberOfExtraJumps < maxNumberOfExtraJumps)
                        multipleJumpsReload.Start();
                }
            }

            AttackAnimationReload.OnUpdate(gameTime);
            if (AttackAnimationReload.State == ReloadState.Finished)
                AttackAnimationReload.CompletedFinishedCheck();
        }
    }
}