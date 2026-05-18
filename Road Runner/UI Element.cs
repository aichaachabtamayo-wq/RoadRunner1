using System;

namespace TemplateVSCode;

public class UI_Element
{
    protected int elementValue;
    protected string name;
    protected int xPos;
    protected int yPos;
    public int ElementValue
    {
        get { return elementValue; }
        set { elementValue = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int XPos
    {
        get { return xPos; }
        set { xPos = value; }
    }
    public int YPos
    {
        get { return yPos; }
        set { yPos = value; }
    }

    public UI_Element(string newName, int newElementValue, int newXPos, int newYPos)
    {
        elementValue = newElementValue;
        name = newName;
        xPos = newXPos;
        yPos = newYPos;
    }

    public void Draw()
    {
        Console.SetCursorPosition(xPos, yPos);
        Console.Write(name + ": " + elementValue);  
    }
}
