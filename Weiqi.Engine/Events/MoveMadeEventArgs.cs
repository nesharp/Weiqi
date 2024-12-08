using Weiqi.Engine.Models;

namespace Weiqi.Engine.Events;

public class PutMadeEventArgs : EventArgs
{
    public Put Put { get; }

    public PutMadeEventArgs(Put put)
    {
        Put = put;
    }
}