using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prime_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean rectGrab;      // has a rectangle been grabbed?
        Rectangle rectGrabbed; // the rectange which was grabbed
        Point rectGrabPos;     // the position the rectangle was grabbed at
        Point rectOriginPos;   // the original position of the rectangle
        Boolean[,] cellOccupied = new Boolean[,] {{false, false, false, false, false, false, false, false},
                                                  {false, true,  false, false, false, false, false, false},
                                                  {false, false, false, true,  false, false, false, false},
                                                  {false, false, false, false, false, true,  false, false},
                                                  {false, false, true,  false, false, false, false, false},
                                                  {false, false, false, false, false, false, false, false},
                                                  {false, false, false, false, false, false, false, false},
                                                  {false, false, false, false, false, false, false, false},};

        private void rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // get the rectangle which was grabbed
            rectGrabbed = (Rectangle)e.OriginalSource;
            // get the original position of the rectangle
            rectOriginPos.X = Canvas.GetLeft(rectGrabbed);
            rectOriginPos.Y = Canvas.GetTop(rectGrabbed);
            // move to top layer
            Canvas.SetZIndex(rectGrabbed, 1);
            // get the position it was grabbed at
            rectGrabPos = e.GetPosition(rectGrabbed);
            // capture the mouse (prevents triggering of events not on the rectangle)
            rectGrabbed.CaptureMouse();
            // set rect grab to true
            rectGrab = true;
        }


        private void rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // set rect grab to false
            rectGrab = false;
            // get the mouse position
            Point mousePos = e.GetPosition(canvas);
            // get the row and column under the mouse
            int row = (int)(mousePos.Y / rectGrabbed.Height);
            int column = (int)(mousePos.X / rectGrabbed.Width);
            // if the row and column under the mouse are an unoccpied cell in the gameboard then...
            // ...mark the original cell unoccupied
            // ...mark the new cell occupied
            // ...move the rect to that cell
            // else return it to it's original position
            if (0 <= column && column <= 7 && 0 <= row && row <= 7 && !cellOccupied[row, column])
            {
                int originRow = (int)(rectOriginPos.Y / rectGrabbed.Height);
                int originColumn = (int)(rectOriginPos.X / rectGrabbed.Width);
                cellOccupied[originRow, originColumn] = false;
                cellOccupied[row, column] = true;
                Canvas.SetLeft(rectGrabbed, column * rectGrabbed.Width);
                Canvas.SetTop(rectGrabbed, row * rectGrabbed.Height);
                checkIfSolved();
            }
            else
            {
                Canvas.SetLeft(rectGrabbed, rectOriginPos.X);
                Canvas.SetTop(rectGrabbed, rectOriginPos.Y);
            }
            // release the mouse (allow events outside the rectangle)
            rectGrabbed.ReleaseMouseCapture();
            // move to bottom layer
            Canvas.SetZIndex(rectGrabbed, 0);
        }


        private void rect_MouseMove(object sender, MouseEventArgs e)
        {
            if (rectGrab)
            {
                // get the position of the mouse
                Point mousePos = e.GetPosition(canvas);
                // move the rect with the mouse relative to where it was grabbed
                Canvas.SetLeft(rectGrabbed, mousePos.X - rectGrabPos.X);
                Canvas.SetTop(rectGrabbed, mousePos.Y - rectGrabPos.Y);
            }
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nope!");
        }


        private void checkIfSolved()
        {
            int firstRow = 0;
            int firstColumn = 0;
            int height = 0;
            int width = 0;
            bool filled = true;

            // find the first row and column
            for (int row = 0; row < 8; row++)
            {
                bool found = false;
                for (int column = 0; column < 8; column++)
                {
                    if (cellOccupied[row, column])
                    {
                        firstRow = row;
                        firstColumn = column;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }

            // find the height
            for (int row = firstRow; row < 8; row++)
            {
                if (cellOccupied[row, firstColumn]) height++;
                else break;
            }

            // find the width
            for (int column = firstColumn; column < 8; column++)
            {
                if (cellOccupied[firstRow, column]) width++;
                else break;
            }

            // check if it's a filled rectangle
            for (int row = firstRow+1; row < firstRow + height; row++)
            {
                for (int column = firstColumn+1; column < firstColumn + width; column++)
                {
                    if (!cellOccupied[row, column])
                    {
                        filled = false;
                        break;
                    }
                }
                if (!filled) break;
            }

            // if won display win message
            if (width > 1 && height > 1 && filled && width * height == 4) MessageBox.Show("You won!");
        }
    }
}
