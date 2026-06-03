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
            if (XPos - 1 == 0) // if moving one step left would hit the left border
            {
                XPos = 1; // keep the player at position 1 so they don't go into the border
            }
            else
            {
                XPos--; // otherwise move one cell to the left
            }
        }
        else if (newKey == ConsoleKey.RightArrow)
        {
            if (XPos + 1 == newWidth - 1) // if moving one step right would hit the right border
            {
                XPos = newWidth - 2; // keep the player just before the border
            }
            else
            {
                XPos++; // otherwise move one cell to the right
            }
        }
        else if (newKey == ConsoleKey.UpArrow)
        {
            if (YPos - 1 == 0) // if moving one step up would hit the top border
            {
                YPos = 1; // keep the player just below the top border
            }
            else
            {
                YPos--; // otherwise move one cell up
            }
        }
        else if (newKey == ConsoleKey.DownArrow)
        {
            if (YPos + 1 == newHeight - 1) // if moving one step down would hit the bottom border
            {
                YPos = newHeight - 2; // keep the player just above the bottom border
            }
            else
            {
                YPos++; // otherwise move one cell down
            }
        }
    }
}
