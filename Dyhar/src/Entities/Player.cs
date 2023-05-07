using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dyhar.src.Mechanics;
using System;

namespace Dyhar.src.Entities
{
    public class Player : MovingGameObject, IWeaponUser
    {
        public static Texture2D sprite;

        bool IsInJump = false;

        int numberOfExtraJumps = 1;
        int maxNumberOfExtraJumps = 3;
        int JumpPower = 15;
        Reload multipleJumpsReload = new Reload("jump_reload", 10);

        public Direction DirectionLook { get; private set; }
        public MeleeWeapon CurrentWeapon { get; set; }
        public Reload attackAnimationReload = new Reload("attack_animation", 500);

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0;
            Force = new Vector2(0, 0);
            IsSolid = false;
            DirectionLook = Direction.Right;
            CurrentWeapon = new Sword(this);
            attackAnimationReload = new Reload("attack_animation", CurrentWeapon.AttackDuration);
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

        public void CheckAllReloads(GameTime gameTime)
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

            attackAnimationReload.OnUpdate(gameTime);
            if (attackAnimationReload.State == ReloadState.Finished)
                attackAnimationReload.CompletedFinishedCheck();
        }

        public override Texture2D GetSprite() => sprite;

        public override void onCollision(GameObject collisionObject)
        {
            return;
        }

        public Vector2 FindWeaponStart()
        {
            if (DirectionLook == Direction.Right)
                return new Vector2((float)(X + SizeSprite.Width), (float)(Y + (SizeSprite.Height / 2)));
            if (DirectionLook == Direction.Left)
                return new Vector2((float)(X), (float)(Y + (SizeSprite.Height / 2)));

            throw new Exception("Impossible Direction");
        }

        public void onAttack()
        {
            attackAnimationReload.Start();
        }

        public bool IsAttacking() => attackAnimationReload.State == ReloadState.Reloading;
        public MeleeWeapon GetCurrentWeapon() => CurrentWeapon;
        public Direction GetDirection() => DirectionLook;
        public Reload GetReload() => attackAnimationReload;
    }
}