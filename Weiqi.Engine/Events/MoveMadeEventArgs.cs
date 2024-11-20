using Weiqi.Engine.Models;

namespace Weiqi.Engine.Events;

public class MoveMadeEventArgs : EventArgs
{
    public Move Move { get; }

    public MoveMadeEventArgs(Move move)
    {
        Move = move;
    }
}