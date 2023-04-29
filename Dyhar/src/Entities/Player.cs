using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Dyhar.src.Mechanics;
using System;

namespace Dyhar.src.Entities
{
    public class Player : MovingGameObject
    {
        public static Texture2D sprite;

        bool IsInJump = false;

        int numberOfExtraJumps = 1;
        int maxNumberOfExtraJumps = 3;
        int JumpPower = 15;
        Reload multipleJumpsReload = new Reload("jump_reload", 1);

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0;
            Force = new Vector2(0, 0);
            IsSolid = false;
        }

        public void MoveHorizontally(Direction direction)
        {
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
                if (multipleJumpsReload.State == ReloadState.NotStarted)
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
        }

        public override Texture2D GetSprite() => sprite;

        public override void onCollision(GameObject collisionObject)
        {
            return;
        }
    }
}