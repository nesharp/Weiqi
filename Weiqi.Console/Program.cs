using System;
using Weiqi.Engine.Models;
using Weiqi.Engine.Players;
using Weiqi.Engine.Game;
using Weiqi.Engine.Events;

namespace Weiqi.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Додаємо UTF-8 для підтримки символів
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Ласкаво просимо до гри Weiqi!");

            // Ініціалізація дошки, гравців і гри
            var board = new Board(19);
            var playerBlack = new ConsolePlayer(Stone.Black);
            var playerWhite = new ConsolePlayer(Stone.White);
            var game = new Game(board, playerBlack, playerWhite);

            // Підписуємося на події
            game.MoveMade += OnMoveMade;
            game.GameEnded += OnGameEnded;

            // Запускаємо гру
            game.Start();
        }

        private static void OnMoveMade(object sender, MoveMadeEventArgs e)
        {
            Console.WriteLine($"Гравець {e.Move.Stone} зробив хід: ({e.Move.Position.X + 1}, {e.Move.Position.Y + 1})");
            
            // Виведення стану дошки після кожного ходу
            if (sender is Game game)
            {
                DisplayBoard(game.Board);
            }
        }

        private static void OnGameEnded(object sender, GameEndedEventArgs e)
        {
            Console.WriteLine("Гра завершена!");
            Console.WriteLine("Дякуємо за гру!");
        }

        /// <summary>
        /// Виводить стан дошки у консоль.
        /// </summary>
        /// <param name="board">Поточний стан дошки</param>
        private static void DisplayBoard(Board board)
        {
            Console.WriteLine("\nПоточний стан дошки:");
            for (int y = 0; y < board.Size; y++)
            {
                for (int x = 0; x < board.Size; x++)
                {
                    var stone = board.GetStone(new Position(x, y));
                    char symbol = stone switch
                    {
                        Stone.Black => '●',  // Чорний камінь
                        Stone.White => '○',  // Білий камінь
                        _ => '.'              // Порожня клітинка
                    };
                    Console.Write($"{symbol} ");
                }
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Гравець, що взаємодіє з консоллю.
    /// </summary>
    public class ConsolePlayer : Player
    {
        public ConsolePlayer(Stone stone) : base(stone) { }

        public override Move MakeMove(Board board)
        {
            while (true)
            {
                Console.WriteLine($"\nХід гравця {Stone}. Введіть координати (формат: x y):");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Пустий ввід. Спробуйте ще раз.");
                    continue;
                }

                var parts = input.Split(' ');
                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int x) ||
                    !int.TryParse(parts[1], out int y) ||
                    x < 1 || x > board.Size || y < 1 || y > board.Size)
                {
                    Console.WriteLine("Невірний формат або координати поза межами дошки. Спробуйте ще раз.");
                    continue;
                }

                var position = new Position(x - 1, y - 1); // Перетворення в 0-індексовану систему

                if (!board.IsPositionEmpty(position))
                {
                    Console.WriteLine("Ця клітинка зайнята. Спробуйте ще раз.");
                    continue;
                }

                return new Move(position, Stone);
            }
        }
    }
}
