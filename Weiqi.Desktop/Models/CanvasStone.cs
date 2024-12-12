using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Weiqi.Engine.Models;

namespace Weiqi.Desktop.Models
{
    public class CanvasStone : StoneRepresentation
    {
        public CanvasStone(BoardCellState boardCellState) : base(boardCellState) { }

        public override void Draw(Canvas canvas, double x, double y, double size)
        {
            Ellipse stoneEllipse = new Ellipse
            {
                Width = size - 4,
                Height = size - 4,
                Fill = BoardCellState == BoardCellState.Black ? Brushes.Black : Brushes.White,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            Canvas.SetLeft(stoneEllipse, x - stoneEllipse.Width / 2);
            Canvas.SetTop(stoneEllipse, y - stoneEllipse.Height / 2);
            canvas.Children.Add(stoneEllipse);
        }
    }
}