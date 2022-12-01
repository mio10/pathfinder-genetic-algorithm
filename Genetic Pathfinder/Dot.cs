using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Genetic_Pathfinder
{
    public abstract class Dot
    {
        protected Point position = new Point(1, 1);
        protected int radius = 10; 
        protected Color color = Color.Black;

        public Point GetPosition()
        {
            return position;
        }

        public Rectangle GetRect()
        {
            return new Rectangle(position.X - (int)(radius / 2), position.Y - (int)(radius / 2), radius, radius);
        }

        public Color GetColor()
        {
            return color;
        }
    }
}
