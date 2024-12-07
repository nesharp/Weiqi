using Weiqi.Engine.Game;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Players;

public class HumanPlayer:Player
{
    public HumanPlayer(BoardCellState boardCellState) : base(boardCellState)
    {
    }
    public override PutCell MakePut(Board board, Position position)
    {
        PutCell putCell = new PutCell(position, this.BoardCellState);
        RulesEngine rulesEngine = new RulesEngine();
        
        if (!rulesEngine.IsMoveLegal(board, putCell))
        {
            return null;
        }
        board.PlaceStone(putCell);
        return new PutCell(position, this.BoardCellState);
    }
}