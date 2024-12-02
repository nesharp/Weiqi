namespace Weiqi.Engine.Models;

public class Board
{
    private readonly Stone[,] _grid;
    public int Size { get; }

    public Board(int size)
    {
        Size = size;
        _grid = new Stone[size, size];
    }

    public Stone GetStone(Position position)
    {
        return _grid[position.X, position.Y];
    }

    public void PlaceStone(Move move)
    {
        _grid[move.Position.X, move.Position.Y] = move.Stone;
    }

    public bool IsPositionEmpty(Position position)
    {
        return GetStone(position) == Stone.None;
    }
    
    public bool PositionIsOnBoard(Position position)
    {
        return position.X >= 0 && position.X < Size && position.Y >= 0 && position.Y < Size;

    }
}