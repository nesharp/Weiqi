using Weiqi.Engine.Game;
using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public class HumanPlayer:Player
{
    public HumanPlayer(BoardCellState boardCellState) : base(boardCellState)
    {
    }

    public override Put MakePut(Board board, IRulesEngine rulesEngine, Position position)
    {
        Put put = new Put(position, this.BoardCellState);
        
        if (!rulesEngine.IsPutLegal(board, put))
        {
            return null;
        }
        rulesEngine.ApplyPut(board, put);
        return put;
    }
}