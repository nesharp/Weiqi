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

    public void PlaceStone(Position position, Stone stone)
    {
        _grid[position.X, position.Y] = stone;
    }

    public bool IsPositionEmpty(Position position)
    {
        return GetStone(position) == Stone.None;
    }
    
    public bool PositionIsOnBoard(Position position)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}