using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeGame
{
    public partial class Form1 : Form
    {
        List<GamePiece> gpList;

        GameBoard gameboard;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gpList = new List<GamePiece>();
            gameboard = new GameBoard();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            Pen blackpen = new Pen(Color.Black);

            List<Rectangle> rectList = new List<Rectangle>();

            

            for (int i = 0; i < gameboard.grid.GetLength(0); i++)
            {
                for (int j = 0; j < gameboard.grid.GetLength(1); j++)
                {
                    //Rectangle rect = new Rectangle(0 + (i * 30), 0 + (j * 30), 30, 30);
                    //rectList.Add(rect);
                    e.Graphics.DrawRectangle(blackpen, gameboard.grid[i, j]);
                    
                    //e.Graphics.DrawRectangle(blackpen, 0 + (i * 30), 0 + (j * 30), 30, 30);
                }
            }

            Rectangle rc = new Rectangle(0, 0, 30, 30);

            System.Drawing.Brush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            e.Graphics.FillRectangle(myBrush, rc);

            GamePiece gp = new GamePiece();
            gp.rect = rc;

            gpList.Add(gp);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(string.Format("X: {0} Y: {1}", e.X, e.Y));
            gameboard.getGridPositionFromXY(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {


        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int oldX = 0;
            int oldY = 0;
            oldX = e.X;
            oldY = e.Y;
            pictureBox1.Refresh();
            Rectangle rc = new Rectangle(e.X-15, e.Y-15, 30, 30);
            System.Drawing.Brush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            pictureBox1.CreateGraphics().FillRectangle(myBrush, rc);
            GamePiece gp = new GamePiece();
            gp.rect = rc;
            gpList.Add(gp);
        }
    }
}
