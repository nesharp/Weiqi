using System.Windows.Controls;
using Weiqi.Engine.Models;

namespace Weiqi.Desktop.Models
{
    public abstract class StoneRepresentation
    {
        protected BoardCellState BoardCellState;

        protected StoneRepresentation(BoardCellState boardCellState)
        {
            this.BoardCellState = boardCellState;
        }

        public abstract void Draw(Canvas canvas, double x, double y, double size);
    }
}