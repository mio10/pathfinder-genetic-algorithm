using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Genetic_Pathfinder
{
    public static class Engine
    {
        public enum Direction
        {
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW
        }

        public const int WIDTH = 860;
        public const int HEIGHT = 445;
        public const int START_X = (int)(WIDTH / 4);
        public const int START_Y = (int)(HEIGHT / 2);
        public static int targetX = (int)(WIDTH * 0.75);
        public static int targetY = (int)(HEIGHT / 2);
        public const int MAXPOP = 100;
        public const int PATHLEN = 1000;
        public const double MUTCHANCE = 0.5;

        public static int countLifetimeCells = 0;
        public static int cellsStepLength = 5;

        public static bool constructionMode = true;
        static bool movesAllowed = false;

        public static List<Dot> dots = new List<Dot>();
        public static List<Dot> obstacles = new List<Dot>();
        public static Random rand = new Random();
        
        public static void Start()
        {
            AddTarget(new Point(targetX, targetY));
            for (int i = 0; i < MAXPOP; i++)
            {
                AddCell(new Point(START_X, START_Y), RandomPath(PATHLEN));
            }
            
            movesAllowed = true;
        }

        public static void SkipGenerations(int count)
        {
            int generation = Algorithm.generationCount;
            while ((Algorithm.generationCount < generation + count) && (!Algorithm.converge))
            {
                CellsMove();
            }
        }
        w
        public static bool TargetLocationVaild(int x, int y)
        {
            if ((x > Engine.WIDTH) || (y > Engine.HEIGHT) ||
                (x < 1) || (y < 1))
            {
                return false;
            }
            return true;
        }

        public static void CellsMove()
        {
            
            foreach (Dot dot in dots)
            {
                if (dot is Cell && movesAllowed)
                {
                    (dot as Cell).Step((dot as Cell).NextMove());
                }
            }
            if (CountDead() == MAXPOP)
            {
                Algorithm.NextGeneration();
            }
        }

        static bool SomeReached()
        {
            foreach (Dot dot in dots)
            {
                if ((dot is Cell) && (dot as Cell).reached)
                    return true;
            }
            return false;
        }

        static int CountDead()
        {
            int count = 0;

            foreach (Dot dot in dots)
            {
                if ((dot is Cell) && (!(dot as Cell).alive))
                {
                    count++;
                }
            }
            return count;
        }

        public static void RemoveCells()
        {
            bool anyCellsLeft = true;
            int count = 0;
            int countRemoved = 0;

            foreach (Dot dot in dots)
            {
                if (dot is Cell)
                {
                    count++;
                }
            }
            
            while (anyCellsLeft)
            {
                int i = 0;
                while (i < dots.Count)
                {
                    if (dots[i] is Cell)
                    {
                        dots.RemoveAt(i);
                        countRemoved++;
                    }
                    i++;
                }
                if (count == countRemoved)
                {
                    anyCellsLeft = false;
                }
            }
        }

        public static bool PointEmpty(Point pt)
        {
            foreach (Dot dot in obstacles)
            {
                if (dot.GetPosition().X == pt.X && dot.GetPosition().Y == pt.Y)
                    return false;
            }
            return true;
        }

        public static List<Direction> RandomPath(int length)
        {
            Direction dir = Direction.N;
            List<Direction> path = new List<Direction>();
            int i = 0;
            while (i < length)
            {
                int repeat = rand.Next(1, 50);
                double random = rand.NextDouble();
                for (int j = 0; j < repeat; j++)
                {
                    if (random < 0.125)
                    {
                        dir = Direction.N;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.125 && random < 0.250)
                    {
                        dir = Direction.NE;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.250 && random < 0.375)
                    {
                        dir = Direction.E;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.375 && random < 0.500)
                    {
                        dir = Direction.SE;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.500 && random < 0.625)
                    {
                        dir = Direction.S;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.625 && random < 0.750)
                    {
                        dir = Direction.SW;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.750 && random < 0.875)
                    {
                        dir = Direction.W;
                        path.Add(dir);
                        continue;
                    }
                    if (random >= 0.875 && random < 1.000)
                    {
                        dir = Direction.NW;
                        path.Add(dir);
                        continue;
                    }
                }
                i += repeat;
            }
            return path;
        }

        public static void AddObstacle(Point pt)
        {
            if (PointEmpty(pt))
                obstacles.Add(new Obstacle(pt));
        }

        public static void AddCell(Point pt, List<Direction> path)
        {
            countLifetimeCells++;
            dots.Add(new Cell(pt, path, countLifetimeCells));
        }

        static void AddTarget(Point pt)
        {
            dots.Add(new Target(pt));
        }
        
    }
}
