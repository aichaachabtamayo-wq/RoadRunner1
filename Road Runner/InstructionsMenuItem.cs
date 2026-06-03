namespace TemplateVSCode;

public class InstructionsMenuItem : MenuItem
{
    public InstructionsMenuItem() : base("How to play")
    {
    }

    public override void Activate(Game game)
    {
        game.CurrentGameState = GameState.Instructions; // go to instructions screen
    }
}