using Weiqi.Engine.Models;

namespace Weiqi.Engine.Events;

public class MoveMadeEventArgs : EventArgs
{
    public PutCell PutCell { get; }

    public MoveMadeEventArgs(PutCell putCell)
    {
        PutCell = putCell;
    }
}