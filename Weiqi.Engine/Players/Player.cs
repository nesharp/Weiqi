using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public abstract class Player : IPlayer
{
   public Stone Stone { get; }
   
   protected Player(Stone stone)
   {
      Stone = stone;
   }

   public abstract Move MakeMove(Board board);
}