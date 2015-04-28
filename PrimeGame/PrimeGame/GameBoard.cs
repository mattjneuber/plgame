using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace PrimeGame
{
    public class GameBoard  ///Gameboard class
    {
        int sizeX = 30;
        int sizeY = 30;

        public Rectangle[,] grid
        {
            get;
            set;
        }
        public Boolean[,] grid_bool
        {
            get;
            set;
        }

        public Rectangle?[,] grid_pieces;

        public GameBoard()
        {
            grid = new Rectangle[8, 8];
            grid_bool = new Boolean[8, 8];
            grid_pieces = new Rectangle?[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = new Rectangle(0 + (i * sizeX), 0 + (j * sizeY), sizeX, sizeY);
                    grid[i, j] = rect;
                    grid_bool[i, j] = false;
                    grid_pieces[i, j] = null;
                }
            }
        }

        public Point getGridPositionFromXY(int X, int Y)
        {
            int newX = X / sizeX;
            int newY = Y / sizeY;

            Console.WriteLine(String.Format("Grid position {0}, {1} contains {2}, {3}", newX, newY, X, Y));
            return new Point(newX, newY);
            //for (int i = 0; i < this.grid.GetLength(0); i++)
            //{
            //    for (int j = 0; j < this.grid.GetLength(1); j++)
            //    {
            //        if (this.grid[i, j].Contains(new Point(X, Y)))
            //        {
            //            Console.WriteLine(String.Format("Grid position {0}, {1} contains {2}, {3}", i, j, X, Y));
            //        }
            //    }
            //}
        }
    }
}
