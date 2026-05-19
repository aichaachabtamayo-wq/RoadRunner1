namespace TemplateVSCode;

public class NameInputScreen
{
    protected string playerName = ""; // starts empty, gets filled as the player types

    public string PlayerName
    {
        get { return playerName; }
    }

    public void Draw()
    {
        Console.SetCursorPosition(5, 5); // set cursos position on screen
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter your name: " + playerName); //show the prompt and what the player is typing
    }

    public void HandleInput(ConsoleKey key, char keyChar)
    {
        if (key == ConsoleKey.Backspace && playerName.Length > 0) //if backspace is pressed and name is not empty
        {
            playerName = playerName.Substring(0, playerName.Length - 1); //remove the last char
        }
        else if (key != ConsoleKey.Enter && playerName.Length < 10) // if not enter and name is shorter than 10 chars
        {
            playerName += keyChar; // add the typed char to the name
        }
    }
}