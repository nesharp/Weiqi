using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Weiqi.Engine.Game
{
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
            var territories = new Dictionary<Stone, HashSet<Position>>()
            {
                { Stone.None, new HashSet<Position>() },
                { Stone.Black, new HashSet<Position>() },
                { Stone.White, new HashSet<Position>() }
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

                    var stone = board.GetStone(position);
                    if (stone != Stone.None)
                    {
                        visited.Add(position);
                        continue;
                    }

                    // Empty position not visited yet
                    var territoryPositions = new HashSet<Position>();
                    var borderingStones = new HashSet<Stone>();

                    var stack = new Stack<Position>();
                    stack.Push(position);
                    visited.Add(position);

                    while (stack.Count > 0)
                    {
                        var current = stack.Pop();
                        territoryPositions.Add(current);

                        foreach (var neighbor in GetNeighboringPositions(board, current))
                        {
                            var neighborStone = board.GetStone(neighbor);
                            if (neighborStone == Stone.None)
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
                    Stone owner;
                    if (borderingStones.Count == 1)
                    {
                        owner = borderingStones.First();
                    }
                    else
                    {
                        owner = Stone.None; // Neutral or disputed territory
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
            var figure = board.GetStone(position);
            if (figure == Stone.None)
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
                    
                    if (Equals(board.GetStone(pos), figure))
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
                    else if (Equals(board.GetStone(pos), Stone.None))
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
                board.PlaceStone(new Move(position, Stone.None));
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

            board.PlaceStone(new Move(position, stone));

            try
            {
                var neighboringEnemyGroups = new List<Group>();
                var neighboringPositions = GetNeighboringPositions(board, position);
                foreach (var p in neighboringPositions)
                {
                    var neighborStone = board.GetStone(p);
                    if (!Equals(neighborStone, stone) && !Equals(neighborStone, Stone.None))
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
                    throw new InvalidOperationException("Move is suicidal");
                }
            }
            catch (InvalidOperationException)
            {
                board.PlaceStone(new Move(position, Stone.None));
                throw;
            }
            
            if (simulate)
            {
                board.PlaceStone(new Move(position, Stone.None));
            }
        }
    }
}
