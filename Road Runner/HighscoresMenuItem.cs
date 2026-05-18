using System;

namespace TemplateVSCode;

public class HighscoresMenuItem : MenuItem
{
    public HighscoresMenuItem() : base("Highscores")
    {
        
    }

    public override void Activate(Game game)
    {
        game.CurrentGameState = GameState.HighscoresMenu;
    }
}
