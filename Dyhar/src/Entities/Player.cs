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

        public double X {  get; private set; }
        public double Y {  get; private set; }
        public Vector2 Position { get => new Vector2((int)X, (int)Y); }
        public SizeF Size { get; private set; }

        public double Speed { get; private set; }
        

        public Player(int x, int y)
        {
            X = x; 
            Y = y;
            Speed = 10.0;
        }

        public void Move(Vector2 moveVector)
        {
            X += moveVector.X;
            Y += moveVector.Y;
        }

        public void Move(double x, double y)
        {
            X += x;
            Y += y;
        }
    }
}
