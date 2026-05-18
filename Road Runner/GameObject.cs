using System;

namespace TemplateVSCode;

public class GameObject
{
    protected string symbol;
    protected ConsoleColor foreColor;

    public string Symbol
    {
        get { return symbol; }
        set { symbol = value; }
    }
    public ConsoleColor ForeColor
    {
        get { return foreColor; }
        set { foreColor = value; }
    }

    public GameObject(string newSymbol, ConsoleColor newForeColor)
    {
        symbol = newSymbol;
        foreColor = newForeColor;
    }
}
