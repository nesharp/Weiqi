namespace Weiqi.Engine.Models;


public class Move
{
    public Position Position { get; }
    public Stone Stone { get; }

    public Move(Position position, Stone stone)
    {
        Position = position;
        Stone = stone;
    }
}
