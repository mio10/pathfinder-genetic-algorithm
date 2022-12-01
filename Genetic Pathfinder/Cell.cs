using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Genetic_Pathfinder
{
    class Cell : Dot
    {
        public List<Engine.Direction> path = new List<Engine.Direction>();
        public bool alive { get; set; }
        public bool reached { get; set; }
        public int step = 0;
        public int id { get; }
        public double invDistance { get; set; }
        public double invSteps { get; set; }
        public double probability { get; set; }

        public Cell(Point pt, List<Engine.Direction> path, int id)
        {
            color = Color.DarkGreen;
            position = pt;
            alive = true;
            reached = false;
            this.path = path;
            this.id = id;
        }

        bool Collision()
        {
            foreach (Dot dot in Engine.obstacles)
            {
                if ((dot is Obstacle) && (Math.Abs(dot.GetPosition().X - position.X) <= (int)(radius * 0.75)) && 
                    (Math.Abs(dot.GetPosition().Y - position.Y) <= (int)(radius * 0.75)))
                {
                    return true;
                }
            }
            if ((position.X <= 1) || (position.Y <= 5) || (position.X >= Engine.WIDTH) || (position.Y >= Engine.HEIGHT))
            {
                return true;
            }
            return false;
        }

        bool TargetReached()
        {
            foreach (Dot dot in Engine.dots)
            {
                if ((dot is Target) && (Math.Abs(dot.GetPosition().X - position.X) <= (int)(radius * 0.75)) &&
                    (Math.Abs(dot.GetPosition().Y - position.Y) <= (int)(radius * 0.75)))
                {
                    return true;
                }
            }
            return false;
        }

        public Engine.Direction NextMove()
        {
            return path[step];
        }

        public void Step(Engine.Direction dir)
        {
            if (Collision())
            {
                alive = false;
                color = Color.Gray;
            }
            if (step >= path.Count - 1)
            {
                alive = false;
                color = Color.Gray;
            }
            if (TargetReached())
            {
                alive = false;
                reached = true;
                color = Color.SkyBlue;
            }
            if (alive)
            {
                step++;
                switch (dir)
                {
                    case Engine.Direction.N:
                        position.Y -= Engine.cellsStepLength;
                        break;
                    case Engine.Direction.NE:
                        position.Y -= Engine.cellsStepLength;
                        position.X += Engine.cellsStepLength;
                        break;
                    case Engine.Direction.E:
                        position.X += Engine.cellsStepLength;
                        break;
                    case Engine.Direction.SE:
                        position.Y += Engine.cellsStepLength;
                        position.X += Engine.cellsStepLength;
                        break;
                    case Engine.Direction.S:
                        position.Y += Engine.cellsStepLength;
                        break;
                    case Engine.Direction.SW:
                        position.Y += Engine.cellsStepLength;
                        position.X -= Engine.cellsStepLength;
                        break;
                    case Engine.Direction.W:
                        position.X -= Engine.cellsStepLength;
                        break;
                    case Engine.Direction.NW:
                        position.Y -= Engine.cellsStepLength;
                        position.X -= Engine.cellsStepLength;
                        break;
                }
            }
        }
    }
}
