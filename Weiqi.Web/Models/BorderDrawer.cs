using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Weiqi.Web.Models
{
    public class BoardDrawer
    {
        private readonly Canvas canvas;
        private readonly int boardSize;
        private readonly double cellSize;

        public BoardDrawer(Canvas canvas, int boardSize, double canvasSize)
        {
            this.canvas = canvas;
            this.boardSize = boardSize;
            this.cellSize = canvasSize / (boardSize - 1);
        }

        public void DrawBoard()
        {
            DrawLines();
            DrawHoshiPoints();
        }

        private void DrawLines()
        {
            for (int i = 0; i < boardSize; i++)
            {
                Line vLine = new Line
                {
                    X1 = i * cellSize,
                    Y1 = 0,
                    X2 = i * cellSize,
                    Y2 = cellSize * (boardSize - 1),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(vLine);

                Line hLine = new Line
                {
                    X1 = 0,
                    Y1 = i * cellSize,
                    X2 = cellSize * (boardSize - 1),
                    Y2 = i * cellSize,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(hLine);
            }
        }

        private void DrawHoshiPoints()
        {
            int[] hoshiPoints = boardSize switch
            {
                19 => new[] { 3, 9, 15 },
                13 => new[] { 3, 6, 9 },
                9 => new[] { 2, 4, 6 },
                _ => Array.Empty<int>()
            };

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
                    canvas.Children.Add(hoshi);
                }
            }
        }
    }
}
