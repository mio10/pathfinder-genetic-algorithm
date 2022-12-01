using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Genetic_Pathfinder
{
    class Obstacle : Dot
    {
        public Obstacle(Point pt)
        {
            position = pt;
        }
    }
}
