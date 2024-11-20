namespace Weiqi.Engine.Models;

public struct Position
{
   public int X { get;  }
   public int Y { get;  }

   public Position(int x, int y)
   {
      X = x;
      Y = y;
   }
}