namespace TemplateVSCode;

public class Highscore
{
    protected string name;
    protected int score;
    protected string color;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public string Color
    {
        get { return color; }
        set { color = value; }
    }

    public Highscore(string newName, int newScore, string newColor)
    {
        Name = newName;
        Score = newScore;
        Color = newColor;
    }
}