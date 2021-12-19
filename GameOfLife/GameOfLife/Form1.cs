using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Label[,] grid;
        Button start;
        Button stop;
        Timer t;
        int gen;
        Label Gen;
        bool[,] alive;
        Button reset;
        Button increase;
        Button decrease;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ActiveCaption;
            grid = new Label[50, 30];
            start = new Button();
            stop = new Button();
            t = new Timer();
            gen = 0;
            Gen = new Label();
            alive = new bool[grid.GetLength(0), grid.GetLength(1)];
            reset = new Button();
            increase = new Button();
            decrease = new Button();

            #region this adds the labels to the form controls
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Label();
                    this.Controls.Add(grid[i, j]);
                    grid[i, j].Click += gridClick;
                }
            }
            #endregion

            #region drawing the grid
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].Size = new Size(20, 20);
                    grid[i, j].Location = new Point(30 + (20 * i), 30 + (20 * j));
                    grid[i, j].BorderStyle = BorderStyle.FixedSingle;
                    grid[i, j].BackColor = Color.LightGray;
                    if (i == 0 | j == 0 | i == grid.GetLength(0) - 1 | j == grid.GetLength(1) - 1)
                    {
                        grid[i, j].BorderStyle = BorderStyle.None;
                        grid[i, j].BackColor = Color.FromArgb(80, 80, 80);
                        grid[i, j].Click -= gridClick;
                    }
                }
            }
            #endregion

            #region adding buttons

            addNewbutton(start, 100, 40, 1100, 100, "Start");
            addNewbutton(stop, 100, 40, 1100, 180, "Stop");
            addNewbutton(reset, 100, 40, 1100, 260, "Reset");
            addNewbutton(increase, 40, 40, 1180, 330, "+");
            addNewbutton(decrease, 40, 40, 1080, 330, "-");
            reset.Click += resetGrid;
            start.Click += startGenertation;
            stop.Click += stopGenertation;
            increase.Click += speedUp;
            decrease.Click += speedDown;

            #endregion

            #region timer controls

            t.Interval = 500;
            t.Enabled = false;
            t.Tick += nextGeneration;

            #endregion

            #region counter label
            this.Controls.Add(Gen);
            Gen.Location = new Point(1100, 340);
            Gen.TextAlign = ContentAlignment.MiddleCenter;
            Gen.Font = new Font(Gen.Font.Name, 12);
            #endregion
        }

        private void startGenertation(object sender, EventArgs e)
        {
            t.Enabled = true;
        }

        private void stopGenertation(object sender, EventArgs e)
        {
            t.Enabled = false;
        }

        private void gridClick(object sender, EventArgs e)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (sender == grid[i, j])
                    {
                        if (grid[i, j].BackColor == Color.LightGray)
                            grid[i, j].BackColor = Color.Red;
                        else
                            grid[i, j].BackColor = Color.LightGray;
                    }
                }
            }
        }

        private void nextGeneration(object sender, EventArgs e)
        {
            
            #region counter
            gen++;
            Gen.Text = gen.ToString();
            #endregion

            #region scan the state of the game and transfer it into a live array

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].BackColor == Color.Red)
                        alive[i, j] = true;
                    else
                        alive[i, j] = false;
                }
            }

            #endregion

            #region calculating the next generation

            int liveCells;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    liveCells = 0;

                    if (i != 0 & j != 0 & i != grid.GetLength(0) - 1 & j != grid.GetLength(1) - 1)
                    {
                        if (grid[i - 1, j - 1].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i - 1, j].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i - 1, j + 1].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i, j - 1].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i, j + 1].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i + 1, j - 1].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i + 1, j].BackColor == Color.Red)
                            liveCells++;
                        if (grid[i + 1, j + 1].BackColor == Color.Red)
                            liveCells++;

                        if (alive[i, j])
                        {
                            if (liveCells != 2 & liveCells != 3)
                                alive[i, j] = false;
                        }
                        else
                        {
                            if (liveCells == 3)
                                alive[i, j] = true;
                        }
                    }
                }
            }

            #endregion

            #region applying the next generation

            for (int i = 1; i < grid.GetLength(0)-1; i++)
            {
                for (int j = 1; j < grid.GetLength(1)-1; j++)
                {
                    if (alive[i, j])
                        grid[i, j].BackColor = Color.Red;
                    else
                        grid[i, j].BackColor = Color.LightGray;
                }
            }

            #endregion
        }

        private void addNewbutton(Button b, int w, int h, int x, int y, string text)
        {
            this.Controls.Add(b);
            b.Text = text;
            b.Size = new Size(w, h);
            b.Location = new Point(x, y);
            b.BackColor = Color.LightGray;
        }

        private void resetGrid(object sender, EventArgs e)
        {
            t.Enabled = false;
            gen = 0;
            Gen.Text = string.Empty;
            for (int i = 1; i < grid.GetLength(0)-1; i++)
            {
                for (int j = 1; j < grid.GetLength(1)-1; j++)
                {
                    grid[i, j].BackColor = Color.LightGray;
                }
            }
        }

        private void speedUp(object sender, EventArgs e)
        {
            if (t.Interval > 100)
                t.Interval -= 100;
        }

        private void speedDown(object sender, EventArgs e)
        {
            if (t.Interval < 1000)
                t.Interval += 100;
        }
    }
}
