using Weiqi.Engine.Game;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public class HumanPlayer:Player
{
    public HumanPlayer(Stone stone) : base(stone)
    {
    }
    public override Move MakeMove(Board board, Position position)
    {
        Move move = new Move(position, this.Stone);
        RulesEngine rulesEngine = new RulesEngine();
        
        if (!rulesEngine.IsMoveLegal(board, move))
        {
            return null;
        }
        board.PlaceStone(move);
        return new Move(position, this.Stone);
    }
}