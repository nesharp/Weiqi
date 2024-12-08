using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Weiqi.Engine.Game
{
    public class RulesEngine : IRulesEngine
    {
        public bool IsPutLegal(Board board, Put put)
        {   
            try
            {
                PlaceStone(board, put.Position, put.BoardCellState, true);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Apply a put to the board
        /// </summary>
        /// <param name="board">Board to apply the put to</param>
        /// <param name="put">Put to apply</param>
        /// <exception cref="InvalidOperationException">Thrown if the put is not legal</exception>
        public void ApplyPut(Board board, Put put)
        {
            if (!IsPutLegal(board, put))
            {
                throw new InvalidOperationException("Put is not legal");
            }
            PlaceStone(board, put.Position, put.BoardCellState);
        }

        public bool IsGameOver(Board board)
        {
            var territories = FindTerritories(board);
            if (territories.TryGetValue(BoardCellState.None, out var emptyTerritory))
            {
                return emptyTerritory.Count == 0;
            }
            return false;
        }

        public int CalculateScore(Board board, BoardCellState boardCellState)
        {
            var territories = FindTerritories(board);

            if (territories.TryGetValue(boardCellState, out var stoneTerritory))
            {
                return stoneTerritory.Count;
            }

            return 0;
        }

        private Dictionary<BoardCellState, HashSet<Position>> FindTerritories(Board board)
        {
            var territories = new Dictionary<BoardCellState, HashSet<Position>>()
            {
                { BoardCellState.None, new HashSet<Position>() },
                { BoardCellState.Black, new HashSet<Position>() },
                { BoardCellState.White, new HashSet<Position>() }
            };

            var visited = new HashSet<Position>();
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    var position = new Position(x, y);
                    if (visited.Contains(position))
                    {
                        continue;
                    }

                    var stone = board.GetCellState(position);
                    if (stone != BoardCellState.None)
                    {
                        visited.Add(position);
                        continue;
                    }

                    // Empty position not visited yet
                    var territoryPositions = new HashSet<Position>();
                    var borderingStones = new HashSet<BoardCellState>();

                    var stack = new Stack<Position>();
                    stack.Push(position);
                    visited.Add(position);

                    while (stack.Count > 0)
                    {
                        var current = stack.Pop();
                        territoryPositions.Add(current);

                        foreach (var neighbor in GetNeighboringPositions(board, current))
                        {
                            var neighborStone = board.GetCellState(neighbor);
                            if (neighborStone == BoardCellState.None)
                            {
                                if (!visited.Contains(neighbor))
                                {
                                    visited.Add(neighbor);
                                    stack.Push(neighbor);
                                }
                            }
                            else
                            {
                                borderingStones.Add(neighborStone);
                            }
                        }
                    }

                    // Determine owner of the territory
                    BoardCellState owner;
                    if (borderingStones.Count == 1)
                    {
                        owner = borderingStones.First();
                    }
                    else
                    {
                        owner = BoardCellState.None; // Neutral or disputed territory
                    }

                    if (!territories.ContainsKey(owner))
                    {
                        territories[owner] = new HashSet<Position>();
                    }

                    foreach (var pos in territoryPositions)
                    {
                        territories[owner].Add(pos);
                    }
                }
            }

            return territories;
        }

        private Group GetGroupAtPosition(Board board, Position position)
        {
            var figure = board.GetCellState(position);
            if (figure == BoardCellState.None)
            {
                throw new InvalidOperationException("Position is empty");
            }

            Group Bfs(Queue<Position> queue, HashSet<Position> visited, Group group)
            {
                while (queue.Count > 0)
                {
                    var pos = queue.Dequeue();
                    if (visited.Contains(pos))
                    {
                        continue;
                    }
                    visited.Add(pos);
                    
                    if (Equals(board.GetCellState(pos), figure))
                    {
                        group.Stones.Add(pos);
                        foreach (var neighbor in GetNeighboringPositions(board, pos))
                        {
                            if (!visited.Contains(neighbor))
                            {
                                queue.Enqueue(neighbor);
                            }
                        }
                    }
                    else if (Equals(board.GetCellState(pos), BoardCellState.None))
                    {
                        group.Liberties.Add(pos);
                    }
                }

                return group;
            }
            
            var group = new Group(figure);
            var visited = new HashSet<Position>();
            var queue = new Queue<Position>();
            queue.Enqueue(position);
            return Bfs(queue, visited, group);
        }

        private void RemoveGroup(Board board, Group group)
        {
            foreach (var position in group.Stones)
            {
                board.PlaceStone(new Put(position, BoardCellState.None));
            }
        }

        private List<Position> GetNeighboringPositions(Board board, Position position)
        {
            var deltas = new List<Position>
            {
                new Position(0, 1),
                new Position(0, -1),
                new Position(1, 0),
                new Position(-1, 0)
            };
            var result = new List<Position>();
            foreach (var delta in deltas)
            {
                var newPosition = position + delta;
                if (board.PositionIsOnBoard(newPosition))
                {
                    result.Add(newPosition);
                }
            }
            return result;
        }

        private void PlaceStone(Board board, Position position, BoardCellState boardCellState, bool simulate = false)
        {
            if (!board.PositionIsOnBoard(position))
            {
                throw new InvalidOperationException("Position is not on board");
            }
            if (!board.IsPositionEmpty(position))
            {
                throw new InvalidOperationException("Position is not empty");
            }

            board.PlaceStone(new Put(position, boardCellState));

            try
            {
                var neighboringEnemyGroups = new List<Group>();
                var neighboringPositions = GetNeighboringPositions(board, position);
                foreach (var p in neighboringPositions)
                {
                    var neighborStone = board.GetCellState(p);
                    if (!Equals(neighborStone, boardCellState) && !Equals(neighborStone, BoardCellState.None))
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


                var newGroup = GetGroupAtPosition(board, position);
                var liberties = newGroup.Liberties;

                if (liberties.Count == 0)
                {
                    throw new InvalidOperationException("Put is suicidal");
                }
            }
            catch (InvalidOperationException)
            {
                board.PlaceStone(new Put(position, BoardCellState.None));
                throw;
            }
            
            if (simulate)
            {
                board.PlaceStone(new Put(position, BoardCellState.None));
            }
        }
    }
}
