using Weiqi.Engine.Models;

namespace Weiqi.Engine.Interfaces;

public interface IPlayer
{
    BoardCellState BoardCellState { get; }
    PutCell MakePut(Board board, Position position);
}