using System;

namespace TemplateVSCode;

public class StartGameMenuItem : MenuItem
{

    public StartGameMenuItem() : base("Start game")
    {
        
    }

    public override void Activate(Game game)
    {
        game.ResetGame(); 
        game.CurrentGameState = GameState.NameInput; // go to name input screen first
    }
}