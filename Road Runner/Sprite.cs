using System;

namespace TemplateVSCode;

public class Sprite : GameObject
{
    protected double xPos; //changed this from int to double
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
        
        // reset color to white so that elements drawn after the player are not affected by the player's color
        Console.ForegroundColor = ConsoleColor.White; 
    }
    public virtual void Move()
    {
        
    }
    public virtual void Update(double dt, int screenWidth, int screenHeight)
    {
        
    }


}
