using Weiqi.Engine.Models;

namespace Weiqi.Engine.Interfaces;

public interface IPlayer
{
    BoardCellState BoardCellState { get; }
    Put MakePut(Board board, Position position);
}