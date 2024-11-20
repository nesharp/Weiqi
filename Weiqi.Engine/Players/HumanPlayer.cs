using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public class HumanPlayer:Player
{
    public HumanPlayer(Stone stone) : base(stone)
    {
    }
    //TODO:todo
    public override Move MakeMove(Board board)
    {
        return null;
    }
}