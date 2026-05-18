using System;

namespace TemplateVSCode;

public enum RoadElementType
{
    Grass,
    Road,
    Border
}
public class RoadElement : GameObject
{
    protected RoadElementType type;
    protected ConsoleColor backColor;
    
    public RoadElementType Type
    {
        get { return type; }
        set { type = value; }
    }
    public ConsoleColor BackColor
    {
        get { return backColor; }
        set { backColor = value; }
    }

    public RoadElement(RoadElementType newType, string newSymbol, ConsoleColor newForeColor, ConsoleColor newBackColor) : base(newSymbol, newForeColor)
    {
        type = newType;
        backColor = newBackColor;
    }

    public void Draw(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = ForeColor;
        Console.BackgroundColor = BackColor;
        Console.Write(Symbol);
    }
}
