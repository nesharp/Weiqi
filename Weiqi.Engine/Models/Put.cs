namespace Weiqi.Engine.Models;


public class Put
{
    public Position Position { get; }
    public BoardCellState BoardCellState { get; }

    public Put(Position position, BoardCellState boardCellState)
    {
        Position = position;
        BoardCellState = boardCellState;
    }
}
