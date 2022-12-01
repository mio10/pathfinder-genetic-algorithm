using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Genetic_Pathfinder
{
    class PenaltyDot : Dot
    {
        public PenaltyDot(Point pt)
        {
            color = Color.Red;
            position = pt;
        }
    }
}
