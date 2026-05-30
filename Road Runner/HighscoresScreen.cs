using System;

namespace TemplateVSCode;

public class HighscoresScreen : Screen // shows highscores
{
    public HighscoresScreen() : base("HighscoresMenuTxt.txt")
    {
        
    }

    public void LoadHighscores(HighscoreManager highscoreManager) // to use the list of highscores
    {
        int y = 15;

        foreach(Highscore hs in highscoreManager.Highscores) // loop through each highscore in the list
        {
            Console.SetCursorPosition(5, y);
            ConsoleColor color;
            Enum.TryParse(hs.Color, out color); // convert string to ConsoleColor
            Console.ForegroundColor = color; //use players color
            Console.Write(hs.Name + " - " + hs.Score); //display name+score of this highscore
            y++;
        }
        Console.ForegroundColor = ConsoleColor.White; // reset color to white so that elements drawn after the highscores are not affected by the player's color
    }
}