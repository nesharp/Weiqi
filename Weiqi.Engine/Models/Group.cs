namespace Weiqi.Engine.Models;

public class Group
{
    public List<Position> Stones { get; }
    public List<Position> Liberties { get; }
    public Stone Stone { get; }
    
    public Group(Stone stone)
    {
        Stone = stone;
        Stones = new List<Position>();
        Liberties = new List<Position>();
    }

    public override bool Equals(object? obj)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        // Need for dictionary key
        // TODO: Implement
        throw new NotImplementedException();
    }
}