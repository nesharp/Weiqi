namespace Weiqi.Engine.Models;

public record struct Position
{
   public int X { get;  }
   public int Y { get;  }

   public Position(int x, int y)
   {
      X = x;
      Y = y;
   }
   
   public static Position operator +(Position a, Position b)
   {
      // TODO: Implement
      throw new NotImplementedException();
   }

   public override int GetHashCode()
   {
      // TODO: Implement
      throw new NotImplementedException();
   }
}