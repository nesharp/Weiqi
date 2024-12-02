namespace Weiqi.Engine.Models;

public class Group
{
    public HashSet<Position> Stones { get; }
    public HashSet<Position> Liberties { get; }
    public Stone Stone { get; }
    
    public Group(Stone stone)
    {
        Stone = stone;
        Stones = new HashSet<Position>();
        Liberties = new HashSet<Position>();
    }

    public override bool Equals(object? obj)
    {
        return obj is Group group &&
               EqualityComparer<HashSet<Position>>.Default.Equals(Stones, group.Stones) &&
               EqualityComparer<HashSet<Position>>.Default.Equals(Liberties, group.Liberties) &&
               Stone == group.Stone;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Stones, Liberties, Stone);
    }
}