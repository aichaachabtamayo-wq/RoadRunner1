using System.Text.Json;

namespace TemplateVSCode;

public class HighscoreManager // saves and loads highscores
{
    protected string filePath = "highscores.json";
    protected List<Highscore> highscores;

    public List<Highscore> Highscores
    {
        get { return highscores; } // return the list of highscores
    }

    public HighscoreManager()
    {
        highscores = new List<Highscore>(); // empty list to store highscores
        LoadHighScores(); // load highscores from JSON file on startup
    }

    public void LoadHighScores() // loads scores from the previous session when the game starts
    {
        StreamReader reader = null; // create an empty reader, no file opened yet
        try
        {
            reader = new StreamReader(filePath); // open the file at the given path
            string json = reader.ReadToEnd(); // read the entire file as one long string
            highscores = JsonSerializer.Deserialize<List<Highscore>>(json); // convert the JSON string to a list of Highscore objects
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // we can only close a file if it is open
            // so we check first whether reader is null or not
            if (reader != null)
            {
                reader.Close(); // always close the file, even after an error
            }
        }
    }

    public void SaveHighScores() // saves the new score after game over
    {
        StreamWriter writer = null;
        try
        {
            string json = JsonSerializer.Serialize(highscores); // convert the list to a JSON string
            writer = new StreamWriter(filePath); // open the file for writing
            writer.Write(json); // write the JSON string to the file
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (writer != null)
            {
                writer.Close(); // always close the file, even after an error
            }
        }
    }

    public void AddHighscore(string newName, int newScore, string newColor)
    {
        highscores.Add(new Highscore(newName, newScore, newColor)); // add the new highscore to the list

        // sort from highest to lowest score
        for (int current = 0; current < highscores.Count - 1; current++) // loop through all scores
        {
            for (int next = current + 1; next < highscores.Count; next++) // compare with all scores after current
            {
                if(highscores[next].Score > highscores[current].Score) // if the next score is higher
                {
                    Highscore temp = highscores[current]; //temp save current score
                    highscores[current] = highscores[next]; // replace with the higher score
                    highscores[next] = temp;
                } 
            }
        }

        //keep only top 5
        while(highscores.Count > 5)
        {
            highscores.RemoveAt(highscores.Count - 1);
        }

        SaveHighScores(); // immediately save to file
    }
}
