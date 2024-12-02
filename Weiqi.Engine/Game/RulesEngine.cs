using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Game;


public class RulesEngine : IRulesEngine
{
    public bool IsMoveLegal(Board board, Move move)
    {   
        try
        {
            PlaceStone(board, move.Position, move.Stone, true);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    /// <summary>
    /// Apply a move to the board
    /// </summary>
    /// <param name="board">Board to apply the move to</param>
    /// <param name="move">Move to apply</param>
    /// <exception cref="InvalidOperationException">Thrown if the move is not legal</exception>
    public void ApplyMove(Board board, Move move)
    {
        if (!IsMoveLegal(board, move))
        {
            throw new InvalidOperationException("Move is not legal");
        }
        PlaceStone(board, move.Position, move.Stone);
    }

    public bool IsGameOver(Board board)
    {
        var territories = FindTerritories(board);
        if (territories.TryGetValue(Stone.None, out var emptyTerritory))
        {
            return emptyTerritory.Count == 0;
        }
        return false;
    }

    public int CalculateScore(Board board, Stone stone)
    {
        var territories = FindTerritories(board);
    
        if (territories.TryGetValue(stone, out var stoneTerritory))
        {
            return stoneTerritory.Count;
        }
        
        return 0;
    }
    
    private Dictionary<Stone, HashSet<Position>> FindTerritories(Board board)
    {
        // Todo: Implement
        // Use BFS or DFS to find territories
        throw new NotImplementedException();
    }
    
    private Group GetGroupAtPosition(Board board, Position position)
    {
        // TODO: Implement
        // BFS or DFS algorithm to find group
        throw new NotImplementedException();
    }

    private void RemoveGroup(Board board, Group group)
    {
        // TODO: Implement
        // Remove by group.Stones
        throw new NotImplementedException();
    }

    private List<Position> GetNeighboringPositions(Board board, Position position)
    {
        var delta = new List<Position>
        {
            new Position(0, 1),
            new Position(0, -1),
            new Position(1, 0),
            new Position(-1, 0)
        };
        var result = new List<Position>();
        foreach (var d in delta)
        {
            var newPosition = position + d;
            if (board.PositionIsOnBoard(newPosition))
            {
                result.Add(newPosition);
            }
        }
        return result;
    }
    
    private void PlaceStone(Board board, Position position, Stone stone, bool simulate = false)
    {
        if (!board.PositionIsOnBoard(position))
        {
            throw new InvalidOperationException("Position is not on board");
        }
        if (!board.IsPositionEmpty(position))
        {
            throw new InvalidOperationException("Position is not empty");
        }
        
        board.PlaceStone(position, stone);
        
        Group newGroup;
        try
        {
            var neighboringEnemyGroups = new List<Group>();
            var neighboringPositions = GetNeighboringPositions(board, position);
            foreach (var p in neighboringPositions)
            {
                var neighborStone = board.GetStone(p);
                if (!neighborStone.Equals(stone) && !neighborStone.Equals(Stone.None))
                {
                    var neighborGroup = GetGroupAtPosition(board, p);
                    if (!neighboringEnemyGroups.Contains(neighborGroup))
                    {
                        neighboringEnemyGroups.Add(neighborGroup);
                    }
                }
            }
            
            if (!simulate)
            {
                foreach (var group in neighboringEnemyGroups)
                {
                    if (group.Liberties.Count == 0)
                    {
                        RemoveGroup(board, group);
                    }
                }
            }
            
            newGroup = GetGroupAtPosition(board, position);
        }
        catch (InvalidOperationException)
        {
            board.PlaceStone(position, Stone.None);
            throw;
        }
        
        var liberties = newGroup.Liberties;

        if (liberties.Count == 0)
        {
            board.PlaceStone(position, Stone.None);
            throw new InvalidOperationException("Move is suicidal");
        }
        
        if (simulate)
        {
            board.PlaceStone(position, Stone.None);
        }
    }
}