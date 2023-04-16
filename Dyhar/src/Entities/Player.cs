using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyhar.src.Entities
{
    public class Player : GameObject
    {
        public static Texture2D sprite;

        public Size Size = new Size(25, 50);
        public Vector2 Force;

        public double Speed { get; private set; }
        

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0;
            Force = new Vector2(0, 0);
        }

        public void Move(Vector2 moveVector)
        {
            Force += moveVector;
        }

        public void Move(double x, double y)
        {
            Force.X += (float)x;
            Force.Y += (float)y;
        }
    }
}