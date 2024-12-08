using Weiqi.Engine.Game;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public class HumanPlayer:Player
{
    public HumanPlayer(BoardCellState boardCellState) : base(boardCellState)
    {
    }
    public override Put MakePut(Board board, Position position)
    {
        Put put = new Put(position, this.BoardCellState);
        RulesEngine rulesEngine = new RulesEngine();
        
        if (!rulesEngine.IsPutLegal(board, put))
        {
            return null;
        }
        board.SetCellState(put);
        return new Put(position, this.BoardCellState);
    }
}