using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Genetic_Pathfinder
{
    class Target : Dot
    {
        public Target(Point pt)
        {
            position = pt;
            color = Color.Blue;
        }

        public void SetPosition(Point pt)
        {
            position = pt;
        }
    }
}
