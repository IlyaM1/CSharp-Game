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

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0f;
            Force = new Vector2(0, 0);
            IsSolid = false;
            directionLook = Direction.Right;
            currentWeapon = new Sword(this);
            attackAnimationReload = new Reload(currentWeapon.AttackDuration);
            multipleJumpsReload = new Reload(10, onMultipleJumpsReloadFinish);
            currentHealthPoints = maxHealthPoints;
        }

        public void MoveHorizontally(Direction direction)
        {
            directionLook = direction;

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

        public override void onCollision(GameObject collisionObject)
        {
            return;
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

            currentHealthPoints -= weapon.Damage;
            if (currentHealthPoints <= 0)
                onDeath();
        }

        public override Texture2D GetSprite() => sprite;
        public MeleeWeapon GetCurrentWeapon() => currentWeapon;
        public Direction GetDirection() => directionLook;
        public Reload GetAnimationReload() => attackAnimationReload;
        public double GetCurrentHp() => currentHealthPoints;






        bool IsInJump = false;
        int numberOfExtraJumps = 1;
        int maxNumberOfExtraJumps = 3;
        int JumpPower = 15;

        Reload multipleJumpsReload;
        double maxHealthPoints = 100.0;
        double currentHealthPoints = 100.0;

        Direction directionLook;
        MeleeWeapon currentWeapon;
        Reload attackAnimationReload;

        private ulong lastAttackNumber = 0;

        void CheckAllReloads(GameTime gameTime)
        {
            multipleJumpsReload.OnUpdate(gameTime);
            attackAnimationReload.OnUpdate(gameTime);
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