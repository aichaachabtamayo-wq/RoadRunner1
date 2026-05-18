namespace TemplateVSCode;

class Program
{
    static void Main(string[] args)
    {
        // create the game
        Game game = new Game(40, 20);

        // start the game loop
        RunGameLoop(game);
        

    }

    protected static void RunGameLoop(Game game)
    {

        int refreshRate = 25;

        Console.CursorVisible = false; //lijntje om te zien waar je bent aan het typen AFGEZET

        /*Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Clear();*/
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch(); //stopwatch die bijhoudt hoelang elke frame duurt
        stopwatch.Start();

        Reset(game);
        game.Draw();
        Console.SetCursorPosition(0, 0);

        while (true)
        {
            double dt = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart(); 
            game.Update(dt);

            while (Console.KeyAvailable) //wordt er op een toets gedrukt?
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                game.MovePlayer(key.Key);
            }

            System.Threading.Thread.Sleep(1000 / refreshRate);// deel waar we wachten

            Reset(game); // na elke frame alles wissen en opnieuw tekenen
            game.Draw();

        }
    }

    protected static void Reset(Game game)
    {
        //Console.BackgroundColor = ConsoleColor.Black;
        //Console.ForegroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(0, 0);
        game.Draw();
        Console.SetCursorPosition(0, 0);
    }
}