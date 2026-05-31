namespace TemplateVSCode;
public enum CollectibleType
{
    Coin,
    Shield,
    Slowmotion
}

public class Collectible : Sprite
{
    protected int points;
    protected CollectibleType type;

    public int Points
    {
        get { return points; }
        set { points = value; }
    }
    public CollectibleType Type
    {
        get { return type; }
        set { type = value; }
    }
    public Collectible(int newPoints, CollectibleType newType, int newX, int newY, string newSymbol, ConsoleColor newColor) : base(newX, newY, newSymbol, newColor)
    {
        points = newPoints;
        type = newType;
    }


}