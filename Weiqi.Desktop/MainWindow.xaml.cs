using System.Windows;
using System.Windows.Input;
using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;
using Weiqi.Engine.Players;
using Weiqi.Desktop.Controllers;
using Weiqi.Desktop.Models;

namespace WeiqiApp
{
    public partial class MainWindow : Window
    {
        private int boardSize = 19;
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
            IPlayer firstPlayer = new HumanPlayer(Stone.Black);
            IPlayer secondPlayer = new HumanPlayer(Stone.White);
            this.gameController = new GameController(BoardCanvas, board, cellSize, firstPlayer, secondPlayer);
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(BoardCanvas);
            gameController.HandleClick(position);
        }
    }
}