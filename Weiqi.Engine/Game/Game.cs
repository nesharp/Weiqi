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

    public event EventHandler<PutMadeEventArgs> PutMade;
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
            var put = _currentPlayer.MakePut(Board, new Position());

            if (put == null)
            {
                // Гравець пасує або здається
                OnGameEnded(new GameEndedEventArgs());
                return;
            }

            if (_rulesEngine.IsPutLegal(Board, put))
            {
                _rulesEngine.ApplyPut(Board, put);
                OnPutMade(new PutMadeEventArgs(put));

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

    protected virtual void OnPutMade(PutMadeEventArgs e)
    {
        PutMade?.Invoke(this, e);
    }

    protected virtual void OnGameEnded(GameEndedEventArgs e)
    {
        GameEnded?.Invoke(this, e);
    }
    
}