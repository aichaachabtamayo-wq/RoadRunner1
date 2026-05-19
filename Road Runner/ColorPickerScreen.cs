namespace TemplateVSCode;

public class ColorPickerScreen
{
    protected ConsoleColor[] colors =
    {
        ConsoleColor.Yellow,
        ConsoleColor.Red,
        ConsoleColor.Green,
        ConsoleColor.Blue,
        ConsoleColor.Cyan,
        ConsoleColor.Magenta
    };

    protected int selectedIndex = 0; // index of the currently selected color

    public ConsoleColor SelectedColor
    {
        get { return colors[selectedIndex]; }
    }

    public void SelectNext()
    {
        if (selectedIndex == colors.Length - 1) // if at the last color, wrap around to the first
        {
            selectedIndex = 0;
        }
        else
        {
            selectedIndex++;
        }
    }

    public void SelectPrevious()
    {
        if(selectedIndex == 0) // if at the first color, wrap around to the last
        {
            selectedIndex = colors.Length - 1;
        } 
        else
        {
            selectedIndex--;
        }
    }

    public void Draw()
    {
        Console.SetCursorPosition(5, 5);
        Console.ForegroundColor = ConsoleColor.White; // set text color to white
        Console.Write("Choose your color:");

        Console.SetCursorPosition(5, 7);

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("< ");

        Console.ForegroundColor = colors[selectedIndex]; // set color to currently selected color
        Console.Write("@"); // draw the player symbol in that color

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" >");

        Console.SetCursorPosition(5, 9);
        Console.Write("Press ENTER to confirm");
    }
}