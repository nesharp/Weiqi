using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public abstract class Player : IPlayer
{
   public BoardCellState BoardCellState { get; }
   
   protected Player(BoardCellState boardCellState)
   {
      BoardCellState = boardCellState;
   }

   public abstract Put MakePut(Board board, Position position);
}