using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Game;


public class RulesEngine : IRulesEngine
{
    public bool IsMoveLegal(Board board, Move move)
    {
        return board.IsPositionEmpty(move.Position);
    }

    public void ApplyMove(Board board, Move move)
    {
        board.PlaceStone(move.Position, move.Stone);
    }

    public bool IsGameOver(Board board)
    {
        return false;
    }

    public int CalculateScore(Board board, Stone stone)
    {
        return 0;
    }
}