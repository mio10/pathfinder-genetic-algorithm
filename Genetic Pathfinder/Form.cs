using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genetic_Pathfinder
{
    public partial class Form : System.Windows.Forms.Form
    {   
        Graphics g;
        Timer cellsMoveTimer = new Timer();

        bool mouseDown = false;

        public Form()
        {
            InitializeComponent();
        }

        private void DrawDots()
        {
            foreach (Dot dot in Engine.dots)
            {
                g.FillEllipse(new SolidBrush(dot.GetColor()), dot.GetRect());
            }
        }
        
        private void DrawObstacles()
        {
            foreach (Dot dot in Engine.obstacles)
            {
                g.FillEllipse(new SolidBrush(dot.GetColor()), dot.GetRect());
            }
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            if (Engine.constructionMode)
            {
                g.DrawString("[ЛКМ] (удерж.) разместить препятствие\n[ПКМ] точка цели", Font, Brushes.Blue, 1, 25);
            } else
            {
                g.DrawString("Поколение: " + Algorithm.generationCount.ToString() +
                    (Algorithm.converge ? "\nСходимость: " + 
                    Algorithm.convergeNumGen.ToString() : ""), Font, Brushes.Blue, 1, 7);
            }
            if (Algorithm.converge)
            {
                buttonSkip.Hide();
            }
                
            DrawDots();
            DrawObstacles();
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            if (Engine.constructionMode && (e.Button == MouseButtons.Right))
            {
                Engine.targetX = e.Location.X;
                Engine.targetY = e.Location.Y;
                foreach (Dot dot in Engine.dots)
                {
                    if (dot is Target)
                    {
                        (dot as Target).SetPosition(e.Location);
                    }
                }
                Invalidate();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Engine.Start();
            cellsMoveTimer.Interval = 5;
            cellsMoveTimer.Tick += CellsMoveTimer_Tick;
            buttonSkip.Hide();
        }

        private void CellsMoveTimer_Tick(object sender, EventArgs e)
        {
            
            Engine.CellsMove();
            this.Invalidate();
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown && Engine.constructionMode && (e.Button == MouseButtons.Left))
            {
                Engine.AddObstacle(e.Location);

                if (Engine.constructionMode)
                    this.Invalidate();
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (Engine.TargetLocationVaild(Engine.targetX, Engine.targetY))
            {
                buttonStart.Hide();
                buttonSkip.Show();
                Engine.constructionMode = false;
                cellsMoveTimer.Start();
            } else
            {
                throw new Exception("Цель размещена неправильно.");
            }
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            buttonSkip.Hide();
            cellsMoveTimer.Stop();
            Engine.SkipGenerations(50);
            cellsMoveTimer.Start();
        }
    }
}
