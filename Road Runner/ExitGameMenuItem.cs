using System;

namespace TemplateVSCode;

public class ExitGameMenuItem : MenuItem
{
    public ExitGameMenuItem() : base("Exit")
    {
        
    }

    public override void Activate(Game game)
    {
        Environment.Exit(0);
    }
}

