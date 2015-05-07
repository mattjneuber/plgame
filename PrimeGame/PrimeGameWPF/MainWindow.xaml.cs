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
using System.Timers;


namespace PrimeGameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean rectGrab;        // has a rectangle been grabbed?
        Rectangle rectGrabbed;   // the rectange which was grabbed
        Point rectGrabPos;       // the position the rectangle was grabbed at
        Point rectOriginPos;     // the original position of the rectangle
        Boolean[,] cellOccupied; // a 2d array representing which cells on the gameboard have pieces in them
        DateTime startTime;      // the time the game started
        Boolean isGamePiecePrime;// is the number of game pieces a prime number
        int num_GamePieces;	 // The total number of game peices
 
	//Left mouse button down action handler.
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

	//Left mouse button release action handler.
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

	//Adjust the rectagle position based on the current mouse position.
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

	//Prime button click handler.
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (isGamePiecePrime == true)
            {
                MessageBox.Show("Yay!", "You win!");
                restart();
            }
            else
            MessageBox.Show("Nope!", "Sorry!");
        }

	//Iterate through gamespace array in order to check if it has been solved automatically.
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

            // if won display win message; show time taken
            if (width > 1 && height > 1 && filled && width * height == num_GamePieces)
            {
                DateTime finishTime = DateTime.Now;
                TimeSpan runTime = finishTime - startTime;
                MessageBox.Show("You won!\n" + "Time Taken: " + runTime.TotalSeconds + " seconds", "Congratulations!");
                restart();
            }
        }

        private void loadIt() { 

	}

        private void canvas_Loaded(object sender, RoutedEventArgs e) {
            Random rnd = new Random();

            // good primes are  3, 5, 7,11,13,17,19,23,29,31,37,41,43,47
            // good comps are   4, 6, 8, 9,10,12,14,15,16,18,20,21,24,25
            // bad  comps are  22
            num_GamePieces = rnd.Next(3, 22);
            
            int[] column = new int[num_GamePieces];
            int[] row = new int[num_GamePieces];

            isGamePiecePrime = isPrime(num_GamePieces);

            // location and color of game pieces
            int[] rows = { rnd.Next(0,8), rnd.Next(0,8), 3, 4 };
            int[] columns = { 1, 3, 5, 2 };
            Color[] colors = { Colors.Blue, Colors.Red, Colors.Green, Colors.Yellow };

            

            for (int i = 0; i < num_GamePieces; i++)
            {
                row[i] = i % 8;
                column[i] = i % 8;
            }

            Shuffle(row);
            Shuffle(column);

            //checks if there is a repeated location and change it's location. Slow with large number of pieces
            for (int i = 1; i < 2; i++) {
                if (check_Location(row, column)) {
                    i--;
                }
            }

                for (int temp = 0; temp < num_GamePieces; temp++) {
                    Console.WriteLine("{0} , {1}", row[temp], column[temp]);
                }

                Console.WriteLine(isGamePiecePrime);

                // mark all cells as unoccupied
                cellOccupied = new Boolean[,] {{false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},
                                           {false, false, false, false, false, false, false, false},};
            
            // create game pieces
            for (int i = 0; i < num_GamePieces; i++) {
                Rectangle rect = new Rectangle();
                rect.Width = 50;
                rect.Height = 50;
                rect.Fill = new SolidColorBrush(colors[rnd.Next(4)]);
                rect.Stroke = new SolidColorBrush(Colors.Black);
                rect.MouseLeftButtonDown += rect_MouseLeftButtonDown;
                rect.MouseLeftButtonUp += rect_MouseLeftButtonUp;
                rect.MouseMove += rect_MouseMove;
                canvas.Children.Add(rect);
                Canvas.SetTop(rect, rect.Height * row[i]);
                Canvas.SetLeft(rect, rect.Width * column[i]);
                cellOccupied[row[i], column[i]] = true;
            }

            // start the timer
            startTime = DateTime.Now;

            Timer timer = new Timer();
            timer.Elapsed += delegate { updateTimer(startTime); };
            timer.Interval = 10;
            timer.Start();
        }
	
	//Update and display timer.
        public void updateTimer(DateTime st) {
            DateTime finishTime = DateTime.Now;
            TimeSpan runTime = finishTime - st;
            String output = String.Format("Timer: {0}:{1:00}.{2:000}", runTime.Minutes, runTime.Seconds, runTime.Milliseconds);
            timerLabel.Dispatcher.BeginInvoke(new Action(() => { timerLabel.Content = output; }));
        }

        //Check if the number of gamePieces is a prime number
        public static Boolean isPrime(int number)
        {
            int boundery = (int)Math.Floor(Math.Sqrt(number));

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= boundery; i++)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
        //Shuffles the row and column of the gamePieces randomly.
        public static void Shuffle(int[] array)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = array.Length; i > 1; i--)
            {
                int j = rnd.Next(i);  // 0 <= j <= i-1
                //swap
                int tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }

        //check to see if there is a repeting location and change is it.
        public static Boolean check_Location(int[] rows, int[] columns)
        {
            Boolean result = false;
            Random rnd = new Random();

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = i + 1; j < columns.Length; j++)
                {
                    if (rows[i] == rows[j] && columns[i] == columns[j])
                    {
                        Console.WriteLine("Same");
                        result = true;
                        columns[j] = rnd.Next(0, 8);
                    }
                }
            }
            return result;
        }

        private void reset_Click(object sender, RoutedEventArgs e) {
            restart();
        }

        private void restart() {
            // There's no reason not to just start the game over again completely, so that's what we'll do
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.Close();
        }
    }
}
