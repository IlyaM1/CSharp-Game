using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Dyhar.src.Physics;
using System;

namespace Dyhar.src.Entities
{
    public class Player : MovingGameObject
    {
        public static Texture2D sprite;

        bool IsInJump = false;

        int NumberOfPossibleJumps = 2;
        int JumpPower = 15;

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0;
            Force = new Vector2(0, 0);
            SizeSprite = new Size(50, 100);
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
            if (!IsInJump || NumberOfPossibleJumps > 0)
            {
                IsInJump = true;
                NumberOfPossibleJumps -= 1;
                Force.Y -= JumpPower;
            }
        }

        public override void OnIsOnGround()
        {
            IsInJump = false;
            //NumberOfPossibleJumps += 1;
        }
    }
}