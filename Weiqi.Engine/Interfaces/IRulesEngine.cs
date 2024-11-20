using Weiqi.Engine.Models;

namespace Weiqi.Engine.Interfaces;


public interface IRulesEngine
{
    bool IsMoveLegal(Board board, Move move);
    void ApplyMove(Board board, Move move);
    bool IsGameOver(Board board);
    int CalculateScore(Board board, Stone stone);
}