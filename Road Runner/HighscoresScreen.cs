using System;

namespace TemplateVSCode;

public class HighscoresScreen : Screen
{
    public HighscoresScreen() : base("HighscoresMenuTxt.txt")
    {
        
    }

    public void LoadHighscores()
    {
        string highscoresAsText = "";
        StreamReader streamReader = null;
        try
        {
            streamReader = new StreamReader("highscores.json"); 

            //read everything from file and parse to text
            //add highscore as text to "highscoresAsText"

            Console.WriteLine(highscoresAsText);
        }
        catch (Exception e)
        {
            Console.WriteLine("     No highscores yet");
        }
        finally
        {
            if(streamReader != null)
            {
                streamReader.Close();
            }
            
        }
    }
}
