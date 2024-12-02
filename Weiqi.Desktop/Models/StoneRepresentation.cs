using System.Windows.Controls;
using Weiqi.Engine.Models;

namespace Weiqi.Desktop.Models
{
    public abstract class StoneRepresentation
    {
        protected Stone stone;

        protected StoneRepresentation(Stone stone)
        {
            this.stone = stone;
        }

        public abstract void Draw(Canvas canvas, double x, double y, double size);
    }
}