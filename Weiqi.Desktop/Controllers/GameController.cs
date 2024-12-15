using System.Windows;
using System.Windows.Controls;
using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;
using Weiqi.Desktop.Models;

namespace Weiqi.Desktop.Controllers
{
    /// <summary>
    /// Manages the game logic, user interactions, and updates the visual representation of the board.
    /// </summary>
    public class GameController
    {
        private readonly Board board;
        private readonly StoneRepresentation blackStoneRepresentation;
        private readonly StoneRepresentation whiteStoneRepresentation;
        private readonly Canvas canvas;
        private readonly double cellSize;
        private readonly IPlayer firstPlayer;
        private readonly IPlayer secondPlayer;
        private readonly BoardDrawer boardDrawer;
        private readonly IRulesEngine rulesEngine;
        private IPlayer currentPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        /// <param name="canvas">The canvas used for rendering the board and stones.</param>
        /// <param name="board">The logical representation of the game board.</param>
        /// <param name="cellSize">The size of a single cell on the board in pixels.</param>
        /// <param name="firstPlayer">The first player (usually Black).</param>
        /// <param name="secondPlayer">The second player (usually White).</param>
        public GameController(Canvas canvas, Board board, double cellSize, IPlayer firstPlayer, IPlayer secondPlayer, BoardDrawer boardDrawer, IRulesEngine rulesEngine)
        {
            this.canvas = canvas;
            this.board = board;
            this.cellSize = cellSize;
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.currentPlayer = firstPlayer;
            this.boardDrawer = boardDrawer;
            this.rulesEngine = rulesEngine;

            blackStoneRepresentation = new CanvasStone(BoardCellState.Black);
            whiteStoneRepresentation = new CanvasStone(BoardCellState.White);
        }

        /// <summary>
        /// Handles the user's click on the board. Determines the cell where the click occurred 
        /// and attempts to place a boardCellState at that location.
        /// </summary>
        /// <param name="position">The coordinates of the mouse click relative to the canvas.</param>
        public void HandleClick(Point position)
        {
            int xIndex = (int)Math.Round(position.X / cellSize);
            int yIndex = (int)Math.Round(position.Y / cellSize);

            if (xIndex >= 0 && xIndex < board.Size && yIndex >= 0 && yIndex < board.Size)
            {
                PlaceStone(xIndex, yIndex);
            }
        }

        /// <summary>
        /// Places a boardCellState at the specified board cell if the cell is not already occupied.
        /// Updates the game board and visual representation.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        private void PlaceStone(int x, int y)
        {
            if (board.GetCellState(new Position(x, y)) != BoardCellState.None)
            {
                MessageBox.Show("The cell is already occupied!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                var put = this.currentPlayer.MakePut(this.board, rulesEngine, new Position(x, y));

                if (put == null)
                {
                    MessageBox.Show("Invalid putCell!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                DrawBoard();
                
                currentPlayer = currentPlayer == firstPlayer ? secondPlayer : firstPlayer;
            }
            catch(Exception e)
            {
                Console.Write(e);
                MessageBox.Show("The cell is already occupied!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            CheckGameOver();
        }

        private void CheckGameOver()
        {
            if (rulesEngine.IsGameOver(board))
            {
                var blackScore = rulesEngine.CalculateScore(board, BoardCellState.Black);
                var whiteScore = rulesEngine.CalculateScore(board, BoardCellState.White);
                var winner = blackScore > whiteScore ? "Black" : "White";
        
                MessageBoxResult result = MessageBox.Show(
                    $"Game Over! Black: {blackScore}, White: {whiteScore}. {winner} wins!",
                    "Game Over",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void DrawBoard()
        {
            boardDrawer.DrawBoard();
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    double xPos = i * cellSize;
                    double yPos = j * cellSize;

                    if (board.GetCellState(new Position(i, j)) == BoardCellState.Black)
                    {
                        blackStoneRepresentation.Draw(canvas, xPos, yPos, cellSize);
                    }
                    else if (board.GetCellState(new Position(i, j)) == BoardCellState.White)
                    {
                        whiteStoneRepresentation.Draw(canvas, xPos, yPos, cellSize);
                    }
                }
            }
        }
    }
}
