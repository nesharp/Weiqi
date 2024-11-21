using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeiqiApp
{
    public partial class MainWindow : Window
    {
        private int boardSize = 19; // Розмір дошки 19x19
        private double canvasSize = 600; // Розмір Canvas
        private double cellSize; // Розмір клітинки

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            DrawBoard();
        }

        private void InitializeBoard()
        {
            cellSize = canvasSize / (boardSize - 1);
            RenderOptions.SetEdgeMode(BoardCanvas, EdgeMode.Aliased);
        }

        private void DrawBoard()
        {
            // Малюємо лінії
            for (int i = 0; i < boardSize; i++)
            {
                // Вертикальні лінії
                Line vLine = new Line
                {
                    X1 = i * cellSize,
                    Y1 = 0,
                    X2 = i * cellSize,
                    Y2 = canvasSize,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                BoardCanvas.Children.Add(vLine);

                // Горизонтальні лінії
                Line hLine = new Line
                {
                    X1 = 0,
                    Y1 = i * cellSize,
                    X2 = canvasSize,
                    Y2 = i * cellSize,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                BoardCanvas.Children.Add(hLine);
            }

            // Малюємо точки хоші
            int[] hoshiPoints;
            if (boardSize == 19)
            {
                hoshiPoints = new int[] { 3, 9, 15 };
            }
            else if (boardSize == 13)
            {
                hoshiPoints = new int[] { 3, 6, 9 };
            }
            else if (boardSize == 9)
            {
                hoshiPoints = new int[] { 2, 4, 6 };
            }
            else
            {
                hoshiPoints = new int[0];
            }

            foreach (int x in hoshiPoints)
            {
                foreach (int y in hoshiPoints)
                {
                    Ellipse hoshi = new Ellipse
                    {
                        Width = 8,
                        Height = 8,
                        Fill = Brushes.Black
                    };
                    Canvas.SetLeft(hoshi, x * cellSize - 4);
                    Canvas.SetTop(hoshi, y * cellSize - 4);
                    BoardCanvas.Children.Add(hoshi);
                }
            }
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(BoardCanvas);
            int xIndex = (int)Math.Round(position.X / cellSize);
            int yIndex = (int)Math.Round(position.Y / cellSize);

            // Перевіряємо, чи координати в межах дошки
            if (xIndex >= 0 && xIndex < boardSize && yIndex >= 0 && yIndex < boardSize)
            {
                PlaceStone(xIndex, yIndex);
            }
        }

        private void PlaceStone(int x, int y)
        {
            // Викликаємо метод двигуна гри для розміщення каменя
            // Наприклад: bool success = gameEngine.PlaceStone(x, y, currentPlayer);

            // Припустимо, що розміщення успішне
            bool success = true; // Замініть на реальний виклик двигуна

            if (success)
            {
                // Малюємо камінь на дошці
                Ellipse stone = new Ellipse
                {
                    Width = cellSize - 4,
                    Height = cellSize - 4,
                    Fill = Brushes.Black // Змініть на колір гравця
                };
                Canvas.SetLeft(stone, x * cellSize - (stone.Width / 2));
                Canvas.SetTop(stone, y * cellSize - (stone.Height / 2));
                BoardCanvas.Children.Add(stone);

                // Змінюємо поточного гравця
                // currentPlayer = (currentPlayer == Player.Black) ? Player.White : Player.Black;
            }
            else
            {
                MessageBox.Show("Неможливо розмістити камінь у цій точці.", "Хід неможливий", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
