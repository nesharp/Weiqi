namespace Weiqi.Engine.Models;

public class Group
{
    public HashSet<Position> Stones { get; }
    public HashSet<Position> Liberties { get; }
    public BoardCellState BoardCellState { get; }
    
    public Group(BoardCellState boardCellState)
    {
        BoardCellState = boardCellState;
        Stones = new HashSet<Position>();
        Liberties = new HashSet<Position>();
    }

    public override bool Equals(object? obj)
    {
        return obj is Group group &&
               EqualityComparer<HashSet<Position>>.Default.Equals(Stones, group.Stones) &&
               EqualityComparer<HashSet<Position>>.Default.Equals(Liberties, group.Liberties) &&
               BoardCellState == group.BoardCellState;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Stones, Liberties, BoardCellState);
    }
}