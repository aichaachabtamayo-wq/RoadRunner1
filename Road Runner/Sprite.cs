using System;

namespace TemplateVSCode;

public class Sprite : GameObject
{
    protected double xPos; //dit verzanderen van int naar double
    protected double yPos;
    public double XPos
    {
        get { return xPos; }
        set { xPos = value; }
    }
    public double YPos
    {
        get { return yPos; }
        set { yPos = value; }
    }
     public int CursorX
    {
        get { return Convert.ToInt32(xPos); }
    }
    public int CursorY
    {
        get { return Convert.ToInt32(yPos); }
    }

    public Sprite(double newXPos, double newYPos, string newSymbol, ConsoleColor newForeColor) : base(newSymbol, newForeColor)
    {
        XPos = newXPos;
        YPos = newYPos;
    }

    public virtual void Draw(double newXPos, double newYPos, string newSymbol, ConsoleColor newForeColor, int xOffset, int yOffset)
    {
        Console.SetCursorPosition(CursorX + xOffset, CursorY + yOffset);
        Console.ForegroundColor = ForeColor;
        Console.Write(Symbol);
    }
    public virtual void Move()
    {
        
    }
    public virtual void Update(double dt, int screenWidth, int screenHeight)
    {
        
    }


}
