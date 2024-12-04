using Weiqi.Engine.Models;

namespace Weiqi.Engine.Interfaces;

public interface IPlayer
{
    Stone Stone { get; }
    Move MakeMove(Board board, Position position);
}