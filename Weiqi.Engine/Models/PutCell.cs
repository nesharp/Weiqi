namespace Weiqi.Engine.Models;


public class PutCell
{
    public Position Position { get; }
    public BoardCellState BoardCellState { get; }

    public PutCell(Position position, BoardCellState boardCellState)
    {
        Position = position;
        BoardCellState = boardCellState;
    }
}
