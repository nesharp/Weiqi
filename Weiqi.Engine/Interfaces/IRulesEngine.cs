using Weiqi.Engine.Models;

namespace Weiqi.Engine.Interfaces;


public interface IRulesEngine
{
    bool IsMoveLegal(Board board, PutCell putCell);
    void ApplyMove(Board board, PutCell putCell);
    bool IsGameOver(Board board);
    int CalculateScore(Board board, BoardCellState boardCellState);
}