using System;

namespace TemplateVSCode;

public class Player : Sprite
{
    protected int lives;
    protected bool shieldActive;
    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }
    public bool ShieldActive
    {
        get { return shieldActive; }
        set { shieldActive = value; }
    }

    public Player(int newLives, bool newShieldActive, int newXPos, int newYPos, string newSymbol, ConsoleColor newForeColor) : base(newXPos, newYPos, newSymbol, newForeColor)
    {
        Lives = newLives;
        ShieldActive = newShieldActive;
    }


    public override void Draw(double newXPos, double newYPos, string newSymbol, ConsoleColor newForeColor, int newXOffset, int newYOffset)
    {
        base.Draw(newXPos, newYPos, newSymbol, newForeColor, newXOffset, newYOffset);
    }

    public void Move(ConsoleKey newKey, int newWidth, int newHeight)
    {
        if (newKey == ConsoleKey.LeftArrow)
        {
            if (XPos - 1 == 0) // als je 1 stap naar links gaat en je botst tegen een muur
            {
                XPos = 1; // dan zet je die terug op 1 zodat je niet in de muur gaat
            }
            else
            {
                 XPos--; //anders beweeg je een vakje naar links
            } 
        }
        else if (newKey == ConsoleKey.RightArrow)
        {
            if (XPos + 1 == newWidth - 1) 
            {
                XPos = newWidth - 2; 
            }
            else
            {
                 XPos++; 
            } 
        }
        else if (newKey == ConsoleKey.UpArrow)
        {
            if (YPos - 1 == 0) 
            {
                YPos = 1; 
            }
            else
            {
                 YPos--; 
            } 
            
        }
        else if (newKey == ConsoleKey.DownArrow)
        {
            if (YPos + 1 == newHeight - 1) 
            {
                YPos = newHeight - 2; 
            }
            else
            {
                 YPos++; 
            } 
        }
    }
}
