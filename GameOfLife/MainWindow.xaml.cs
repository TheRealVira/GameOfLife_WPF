using System;
using System.Threading;
using System.Windows;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int HEARTBEAT = 111;

        private static Cell[,]cells = new Cell[20,20];

        private Timer GameHeartBeat = new Timer(UpdateGame, null, int.MaxValue, HEARTBEAT);

        private static void UpdateGame(object state)
        {
            foreach (var cell in cells)
            {
                cell.Update();
            }

            foreach (var cell in cells)
            {
                cell.ApplyUpdate();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            var d0 = cells.GetLength(0);
            var d1 = cells.GetLength(1);

            // Initialize Cells
            for (var i = 0; i < d0; i++)
            {
                for (var j = 0; j < d1; j++)
                {
                    cells[i, j] = new Cell();
                    
                    var margin = cells[i, j].Margin;
                    margin.Left = i * 30;
                    margin.Top = j * 30;
                    cells[i, j].Margin = margin;
                    GameOfLifeGrid.Children.Add(cells[i, j]);
                }
            }

            // Set neighbours
            for (var i = 0; i < d0; i++)
            {
                for (var j = 0; j < d1; j++)
                {
                    cells[i, j].SetNeighbours(new Cell[]
                    {
                        cells[mod(i - 1, d0), mod(j - 1, d0)],
                        cells[mod(i, d0), mod(j - 1, d0)],
                        cells[mod(i + 1, d0), mod(j - 1, d0)],
                        cells[mod(i - 1, d0), mod(j, d0)],
                        cells[mod(i + 1, d0), mod(j, d0)],
                        cells[mod(i - 1, d0), mod(j + 1, d0)],
                        cells[mod(i, d0), mod(j + 1, d0)],
                        cells[mod(i + 1, d0), mod(j + 1, d0)]
                    });
                }
            }
        }

        int mod(int x, int m) {
            return (x%m + m)%m;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            playpause_btn.Content = playpause_btn.Content.Equals("Play") ? "Pause" : "Play";

            GameHeartBeat.Change(playpause_btn.Content.Equals("Pause") ? 0 : int.MaxValue, HEARTBEAT);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            foreach (var cell in cells)
            {
                cell.SetState(rand.Next(2) == 1 ? StateOfLife.Alive : StateOfLife.Dead);
            }
        }
    }
}
