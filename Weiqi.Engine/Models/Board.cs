namespace Weiqi.Engine.Models;

public class Board
{
    private readonly BoardCellState[,] _grid;
    public int Size { get; }

    public Board(int size)
    {
        Size = size;
        _grid = new BoardCellState[size, size];
    }

    public BoardCellState GetCellState(Position position)
    {
        return _grid[position.X, position.Y];
    }

    public void SetCellState(Put put)
    {
        _grid[put.Position.X, put.Position.Y] = put.BoardCellState;
    }

    public bool IsPositionEmpty(Position position)
    {
        return GetCellState(position) == BoardCellState.None;
    }
    
    public bool PositionIsOnBoard(Position position)
    {
        return position.X >= 0 && position.X < Size && position.Y >= 0 && position.Y < Size;

    }
}