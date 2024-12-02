namespace Weiqi.Engine.Models;

public record struct Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
   
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}