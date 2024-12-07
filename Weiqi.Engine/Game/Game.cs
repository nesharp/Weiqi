using Weiqi.Engine.Events;
using Weiqi.Engine.Interfaces;
using Weiqi.Engine.Models;

namespace Weiqi.Engine.Game;
//This about removing this class, because idn how to implement this one 
public class Game
{
    public Board Board { get; }
    private readonly IPlayer _playerBlack;
    private readonly IPlayer _playerWhite;
    private readonly IRulesEngine _rulesEngine;

    public event EventHandler<MoveMadeEventArgs> MoveMade;
    public event EventHandler<GameEndedEventArgs> GameEnded;

    private IPlayer _currentPlayer;

    public Game(Board board, IPlayer playerBlack, IPlayer playerWhite)
    {
        Board = board;
        _playerBlack = playerBlack;
        _playerWhite = playerWhite;
        _rulesEngine = new RulesEngine();
        _currentPlayer = _playerBlack;
    }

    public void Start()
    {
        while (!_rulesEngine.IsGameOver(Board))
        {
            var move = _currentPlayer.MakePut(Board, new Position());

            if (move == null)
            {
                // Гравець пасує або здається
                OnGameEnded(new GameEndedEventArgs());
                return;
            }

            if (_rulesEngine.IsMoveLegal(Board, move))
            {
                _rulesEngine.ApplyMove(Board, move);
                OnMoveMade(new MoveMadeEventArgs(move));

                // Зміна гравця
                _currentPlayer = _currentPlayer == _playerBlack ? _playerWhite : _playerBlack;
            }
            else
            {
                // Хід нелегальний, можна повідомити гравця
            }
        }

        OnGameEnded(new GameEndedEventArgs());
    }

    protected virtual void OnMoveMade(MoveMadeEventArgs e)
    {
        MoveMade?.Invoke(this, e);
    }

    protected virtual void OnGameEnded(GameEndedEventArgs e)
    {
        GameEnded?.Invoke(this, e);
    }
    
}