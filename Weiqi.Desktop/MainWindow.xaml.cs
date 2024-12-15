using System.Windows;
using System.Windows.Input;
using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;
using Weiqi.Engine.Players;
using Weiqi.Desktop.Controllers;
using Weiqi.Desktop.Models;
using Weiqi.Engine.Game;

namespace WeiqiApp
{
    public partial class MainWindow : Window
    {
        private int boardSize = 6;
        private double canvasSize = 600;
        private double cellSize;
        private GameController gameController;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.cellSize = canvasSize / (boardSize - 1);

            var boardDrawer = new BoardDrawer(BoardCanvas, boardSize, canvasSize);
            boardDrawer.DrawBoard();

            Board board = new Board(boardSize);
            RulesEngine rulesEngine = new RulesEngine();
            IPlayer firstPlayer = new HumanPlayer(BoardCellState.Black);
            IPlayer secondPlayer = new HumanPlayer(BoardCellState.White);
            this.gameController = new GameController(BoardCanvas, board, cellSize, firstPlayer, secondPlayer, boardDrawer, rulesEngine);
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(BoardCanvas);
            gameController.HandleClick(position);
        }
    }
}