using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Genetic_Pathfinder
{
    static class Algorithm
    {
        public static int generationCount = 1;
        static double sumInvDistances = 0;
        static double sumDistances = 0;
        static double sumInvSteps = 0;

        public static bool converge = false;
        public static int convergeNumGen = 0;
        static List<Cell> newCells = new List<Cell>();
        
        static void CalculateDistancesFromTarget()
        {
            sumInvDistances = 0;
            sumDistances = 0;
            foreach (Dot dot in Engine.dots)
            {
                if (dot is Cell)
                {
                    (dot as Cell).invDistance = 1 /
                        (Math.Sqrt(Math.Pow(dot.GetPosition().X - Engine.targetX, 2) + Math.Pow(dot.GetPosition().Y - Engine.targetY, 2)));
                    sumInvDistances += (dot as Cell).invDistance;
                    sumDistances += 1 / (dot as Cell).invDistance;
                }
            }
        }

        static void CalculateSteps()
        {
            sumInvSteps = 0;
            int countReached = 0;
            int sumSteps = 0;
            foreach (Dot dot in Engine.dots)
            {
                if ((dot is Cell) && (dot as Cell).reached)
                {
                    (dot as Cell).invSteps = 1.0 / (Engine.PATHLEN - (dot as Cell).step);
                    sumInvSteps += (dot as Cell).invSteps;
                    countReached++;
                    sumSteps += (dot as Cell).step;
                }
                if ((dot is Cell) && !(dot as Cell).reached)
                {
                   (dot as Cell).invSteps = 1.0 / (Engine.PATHLEN - (dot as Cell).step);
                   sumInvSteps += (dot as Cell).invSteps;
                   sumSteps += (dot as Cell).step;
                }
            }
            Console.WriteLine(countReached + " достигло цели. Среднее расстояние " + (int)(sumDistances / Engine.MAXPOP));
        }

        static void CalculateProbabilities()
        {
            foreach (Dot dot in Engine.dots)
            {
                if (dot is Cell)
                {
                    (dot as Cell).probability += (dot as Cell).invDistance / sumInvDistances;
                }
            }
        }

        static List<Engine.Direction> MutatePath(List<Engine.Direction> path, int step)
        {
            
            if (Engine.rand.NextDouble() < Engine.MUTCHANCE)
            {
                int count = path.Count;
                path.RemoveRange(step, count - step);
                List<Engine.Direction> newPath = new List<Engine.Direction>();
                newPath = Engine.RandomPath(count - step);
                path.AddRange(newPath);
            }
            
            return path;
        }

        static void CreateChild(Dot d1, Dot d2)
        {
            List<Engine.Direction> path = new List<Engine.Direction>();
            int crossPoint = Engine.rand.Next(1, Engine.PATHLEN - 2);
            path.AddRange((d1 as Cell).path.GetRange(0, crossPoint));
            path.AddRange((d2 as Cell).path.GetRange(crossPoint, Engine.PATHLEN - crossPoint));
            try
            { 
                path = MutatePath(path, Engine.rand.Next(2, Engine.PATHLEN - 1));
                path = MutatePath(path, (d1 as Cell).step - 5);
            }
            catch
            {

            }
            Engine.countLifetimeCells++;
            newCells.Add(new Cell(new System.Drawing.Point(Engine.START_X, Engine.START_Y), path, Engine.countLifetimeCells));
        }

        static void Breed()
        {
            for (int i = 0; i < Engine.MAXPOP; i++)
            {
                Cell first = new Cell(new System.Drawing.Point(1, 1), Engine.RandomPath(1), 0);
                Cell second = new Cell(new System.Drawing.Point(1, 1), Engine.RandomPath(1), 0);

                bool firstIsChosen = false;
                bool secondIsChosen = false;

                while (!firstIsChosen)
                {
                    foreach (Dot dot in Engine.dots)
                    {
                        if ((dot is Cell) && ((dot as Cell).probability > Engine.rand.NextDouble()))
                        {
                            first = (dot as Cell);
                            firstIsChosen = true;
                            break;
                        }
                    }
                }

                while (!secondIsChosen)
                {
                    foreach (Dot dot in Engine.dots)
                    {
                        if ((dot is Cell) && ((dot as Cell).probability > Engine.rand.NextDouble()) && ((dot as Cell) != first))
                        {
                            second = (dot as Cell);
                            secondIsChosen = true;
                            break;
                        }
                    }
                }
                CreateChild(first, second);
            }
            
        }

        public static bool CheckConvergence()
        {
            foreach (Dot dot in Engine.dots)
            {
                if ((dot is Cell) && (dot as Cell).reached)
                {
                    if (!converge)
                    {
                        convergeNumGen = generationCount;
                    }
                    return true;
                }
            }
            return false;
        }

        public static void NextGeneration()
        {
            converge = CheckConvergence();
            newCells.Clear();
            generationCount++;
            CalculateDistancesFromTarget();
            CalculateSteps();
            CalculateProbabilities();
            Breed();
            Engine.RemoveCells();
            Engine.dots.AddRange(newCells);
            Console.WriteLine("Поколение " + generationCount);
        }
    }
}
