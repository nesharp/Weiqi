using Weiqi.Engine.Models;

namespace Weiqi.Engine.Interfaces;


public interface IRulesEngine
{
    bool IsPutLegal(Board board, Put put);
    void ApplyPut(Board board, Put put);
    bool IsGameOver(Board board);
    double CalculateScore(Board board, BoardCellState boardCellState);
}