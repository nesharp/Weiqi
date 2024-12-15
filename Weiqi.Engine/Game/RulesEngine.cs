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
            if (!IsValidPosition(board, put.Position))
            {
                return false;
            }
            
            if (IsGameOver(board))
            {
                Console.WriteLine("Game is over");
                return false;
            }
            
            // Copy the board for simulation
            var boardCopy = CopyBoard(board);

            try
            {
                boardCopy.SetCellState(put);
                
                var neighboringEnemyGroups = GetNeighboringEnemyGroups(boardCopy, put.Position);
                foreach (var group in neighboringEnemyGroups)
                {
                    if (group.Liberties.Count == 0)
                    {
                        RemoveGroup(boardCopy, group);
                    }
                }

                var newGroup = GetGroupAtPosition(boardCopy, put.Position);
                var liberties = newGroup.Liberties;
                if (liberties.Count == 0)
                {
                    return false;
                }
            
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
            
            board.SetCellState(put);
            
            var neighboringEnemyGroups = GetNeighboringEnemyGroups(board, put.Position);
            foreach (var group in neighboringEnemyGroups)
            {
                if (group.Liberties.Count == 0)
                {
                    RemoveGroup(board, group);
                }
            }
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

        public double CalculateScore(Board board, BoardCellState boardCellState)
        {
            var komi = 6.5;
            
            var territories = FindTerritories(board);
            
            if (territories.TryGetValue(boardCellState, out var stoneTerritory))
            {
                if (boardCellState == BoardCellState.White)
                {
                    return stoneTerritory.Count + komi;
                } 
                else
                {
                    return stoneTerritory.Count;
                }
            }
            
            return 0;
        }

        
        private Board CopyBoard(Board board)
        {
            var boardCopy = new Board(board.Size);
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    var position = new Position(x, y);
                    boardCopy.SetCellState(new Put(position, board.GetCellState(position)));
                }
            }

            return boardCopy;
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
            
            int blackStoneCount = 0;
            int whiteStoneCount = 0;

            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    var position = new Position(x, y);
                    var stone = board.GetCellState(position);
                    if (stone == BoardCellState.Black) blackStoneCount++;
                    if (stone == BoardCellState.White) whiteStoneCount++;
                }
            }
            
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
                    
                    var territoryPositions = new HashSet<Position>();
                    var borderingCellStates = new HashSet<BoardCellState>();

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
                                borderingCellStates.Add(neighborStone);
                            }
                        }
                    }
                    
                    BoardCellState owner;
                    if (borderingCellStates.Count == 1)
                    {
                        owner = borderingCellStates.First();
                    }
                    else
                    {
                        owner = BoardCellState.None;
                    }

                    // If there is only one stone of a color on the board, the territory is not owned by that color
                    if (owner == BoardCellState.Black && blackStoneCount == 1)
                    {
                        owner = BoardCellState.None;
                    }
                    if (owner == BoardCellState.White && whiteStoneCount == 1)
                    {
                        owner = BoardCellState.None;
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
            
            var maxCountStoneInBoard = board.Size * board.Size - 1;

            if (territories[BoardCellState.Black].Count == maxCountStoneInBoard)
            {
                territories[BoardCellState.Black].Clear();
            }
            else if (territories[BoardCellState.White].Count == maxCountStoneInBoard)
            {
                territories[BoardCellState.White].Clear();
            }

            return territories;
        }


        private Group GetGroupAtPosition(Board board, Position position)
        {
            var cellState = board.GetCellState(position);
            if (cellState == BoardCellState.None)
            {
                Console.WriteLine("Position is empty");
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
                    
                    if (Equals(board.GetCellState(pos), cellState))
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
            
            var group = new Group(cellState);
            var visited = new HashSet<Position>();
            var queue = new Queue<Position>();
            queue.Enqueue(position);
            return Bfs(queue, visited, group);
        }

        private void RemoveGroup(Board board, Group group)
        {
            foreach (var position in group.Stones)
            {
                board.SetCellState(new Put(position, BoardCellState.None));
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
        
        private List<Group> GetNeighboringEnemyGroups(Board board, Position position)
        {
            var result = new List<Group>();
            var neighboringPositions = GetNeighboringPositions(board, position);
            foreach (var p in neighboringPositions)
            {
                var neighborCellState = board.GetCellState(p);
                if (!Equals(neighborCellState, board.GetCellState(position)) && !Equals(neighborCellState, BoardCellState.None))
                {
                    var neighborGroup = GetGroupAtPosition(board, p);
                    if (!result.Contains(neighborGroup))
                    {
                        result.Add(neighborGroup);
                    }
                }
            }

            return result;
        }
        
        private static bool IsValidPosition(Board board, Position position)
        {
            if (!board.PositionIsOnBoard(position))
            {
                return false;
            }
            if (!board.IsPositionEmpty(position))
            {
                return false;
            }
            
            return true;
        }
    }
}
