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
        int refreshRate = 60;

        Console.CursorVisible = false; // hide the cursor so it doesn't appear while typing

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch(); // stopwatch that tracks how long each frame takes
        stopwatch.Start();

        game.Draw();
        Console.SetCursorPosition(0, 0);

        while (true)
        {
            double dt = stopwatch.Elapsed.TotalSeconds; // time since last frame in seconds
            stopwatch.Restart();
            game.Update(dt);

            while (Console.KeyAvailable) // check if a key is being pressed
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                game.MovePlayer(key.Key, key.KeyChar);
            }

            Thread.Sleep(1000 / refreshRate); // wait to control the frame rate

            game.Draw();
        }
    }
}
