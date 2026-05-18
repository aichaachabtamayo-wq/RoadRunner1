using System.Diagnostics;

namespace TemplateVSCode;

public enum GameState
{
    StartingScreen,
    MainMenu,
    HighscoresMenu,
    GameRunning,
    GamePaused,
    GameOver,
    NoGameState
}
public class Game
{

    protected int width, height;
    protected GameState currentGameState;
    protected GameState previousGameState;
    protected int uiXOffset = 3;
    protected int uiYOffset = 5;
    protected UI gameUI;
    protected UI_Element uiScore, uiTime, uiLives;
    protected Road road;
    protected Player player;
    protected Stopwatch stopwatch;
    protected Screen startingScreen;
    protected Screen gameOverScreen;
    protected Menu mainMenu;
    protected HighscoresScreen highScoresMenu;
    protected StartGameMenuItem startGameMenuItem;
    protected HighscoresMenuItem highscoresMenuItem;
    protected ExitGameMenuItem exitGameMenuItem;
    protected bool gameStarted = false; 
    protected double scrollTimer = 0;
    protected double scrollSpeed = 3;
    protected int lastSafeRow = 18;
    protected double moveTimer = 0;
    protected double moveSpeed = 0.2; 

    public GameState CurrentGameState
    {
        get { return currentGameState; }
        set { currentGameState = value; }
    }

    public Game(int newWidth, int newHeight)
    {

        // set the size
        width = newWidth;
        height = newHeight;

        // set the window
        Console.WindowWidth = width + 1 + uiXOffset;
        Console.WindowHeight = height + 1 + uiYOffset;

        currentGameState = GameState.StartingScreen;
        previousGameState = GameState.NoGameState;


        road = new Road(40);
        player = new Player(3, false, 20, 18, "@", ConsoleColor.Yellow);

        gameUI = new UI();
        uiScore = new UI_Element("Score", 0, 2, 1);
        uiTime = new UI_Element("Time", 0, 15, 1);
        uiLives = new UI_Element("Lives", 3, 28, 1);
        

        gameUI.Add(uiScore);
        gameUI.Add(uiTime);
        gameUI.Add(uiLives);

        stopwatch = new Stopwatch();
        stopwatch.Start();

        startingScreen = new Screen("StartingScreenTxt.txt");
        gameOverScreen = new Screen("GameOverScreenTxt.txt");
        mainMenu = new Menu("MainMenuTxt.txt", ConsoleColor.White, ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkRed);
        highScoresMenu = new HighscoresScreen();
        startGameMenuItem = new StartGameMenuItem();
        highscoresMenuItem = new HighscoresMenuItem();
        exitGameMenuItem = new ExitGameMenuItem();

        mainMenu.AddMenuItem(startGameMenuItem);
        mainMenu.AddMenuItem(highscoresMenuItem);
        mainMenu.AddMenuItem(exitGameMenuItem);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public void Draw()
    {
        switch (currentGameState)
        {
            case GameState.StartingScreen:
                if (currentGameState != previousGameState)
                {
                    startingScreen.Draw();
                }
                break;
            case GameState.MainMenu:
                if (currentGameState != previousGameState || mainMenu.DidActiveMenuItemChange())
                {
                    mainMenu.Draw();
                }
                break;
            case GameState.HighscoresMenu:
                if (currentGameState != previousGameState)
                {
                    highScoresMenu.Draw(); //dit is dus de titel
                    highScoresMenu.LoadHighscores(); //en dit is de 'lijst' met high scores of zeggen dat er geen highscores zijn
                }
                break;
            case GameState.GameRunning:

                road.Draw(uiXOffset, uiYOffset);

                foreach (Vehicle vehicle in road.Vehicles)
                {
                    vehicle.Draw(vehicle.XPos, vehicle.YPos, vehicle.Symbol, vehicle.ForeColor, uiXOffset, uiYOffset);
                }

                player.Draw(player.XPos, player.YPos, player.Symbol, player.ForeColor, uiXOffset, uiYOffset);
                gameUI.Draw();
                break;

            case GameState.GameOver:
                if (currentGameState != previousGameState)
                {
                    gameOverScreen.Draw();
                }
                break;
        }

        previousGameState = currentGameState;

    }

    public void MovePlayer(ConsoleKey key)
    {
        switch (currentGameState)
        {
            case GameState.StartingScreen:
                if (key == ConsoleKey.Enter)
                {
                    ResetScreen();
                    currentGameState = GameState.MainMenu;
                }
                break;
            case GameState.MainMenu:
                if (key == ConsoleKey.Enter)
                {
                    ResetScreen();
                    mainMenu.ActivateMenuItem(this); //object van de klasse waar ik momenteel in zit
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    mainMenu.SelectPreviousItem();
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    mainMenu.SelectNextItem();
                }
                break;
            case GameState.HighscoresMenu:
                if (key == ConsoleKey.Backspace)
                {
                    currentGameState = GameState.MainMenu;
                }
                break;
            case GameState.GameRunning:

                if (moveTimer >= moveSpeed)
                {
                    player.Move(key, width, height);
                    gameStarted = true;
                    moveTimer = 0; //reset timer
                }
                break;

            case GameState.GameOver:
                if (key == ConsoleKey.Spacebar)
                {
                    ResetScreen();
                    currentGameState = GameState.MainMenu;
                }
                break;

        }
    }

    public void Update(double dt)
    {
        switch (currentGameState)
        {
            case GameState.StartingScreen:
                break;

            case GameState.GameRunning:
                foreach (Vehicle vehicle in road.Vehicles)
                {
                    vehicle.Update(dt, width, height);
                }
                gameUI.UpdateUIElementValue("Time", (int)stopwatch.ElapsedMilliseconds / 1000);
                gameUI.UpdateUIElementValue("Lives", player.Lives);
                
                //check welke road rij de speler zich op bevindt
                //Player.YPos is de schermpos, maar road.Rows begint op index 0
                //daarom player.YPos - uiYOffset om de juiste road rij index te krijge

                bool hit = CheckCollision(); //check of speler is geraakt door voertuig

                if (hit)//als speler geraakt wordt
                {
                    player.Lives--;// een leven aftrekken
                    player.YPos =  lastSafeRow; // opgeslagen schermpositie gebruiken
                    player.XPos = 20; //respawn op midden vh scherm
                    
                    if(player.Lives == 0) // als de speler geen levens meer heeft
                    {
                        currentGameState = GameState.GameOver; // ga naar game over screen
                    }
                }
                
                moveTimer += dt; //timer ophogen bij elke frame

                if(gameStarted) //alleen als de speler al bewogen heeft
                {
                    scrollTimer += dt; //timer start
                    
                    if(scrollTimer >= scrollSpeed) //als de timer de speed bereikt
                    {
                        road.Scroll(); //scroll de map
                        if(player.YPos < height - 2) // als de speler nog niet op de laatste rij zit (rij voor de onderste border dus)
                        {
                            //verschuift player 1 omlaag ==> zo blijft speler visueel op dezelfde rij staan als map scrollt
                            player.YPos++;
                            lastSafeRow++;
                        }
                        scrollTimer = 0; //en reset de timer
                    }
                }

                int playerRowIndex = (int)player.YPos - uiYOffset; 

                //check of die rij binnen de grenzen van road valt
                if(playerRowIndex >= 0 && playerRowIndex < road.Rows.Count)
                {
                    //check of de speler op een grass rij staat
                    if(road.Rows[playerRowIndex].Type == RoadElementType.Grass)
                    {
                        lastSafeRow = (int)player.YPos; //sla schermpositie op
                    }
                }
                break;
        }

    }

    public bool CheckCollision()
    {
        bool hit = false;
        foreach (Vehicle car in road.Vehicles)
        {
            if ((int)player.YPos == (int)car.YPos) //checken of ze op dezelfde rij zitten
            {
                if ((int)player.XPos <= (int)car.XPos + car.Symbol.Length - 1 && (int)player.XPos >= (int)car.XPos) //checken of de player binnen de breedte vh voertuig zit
                {
                    hit = true;
                }
            }
        }
        return hit;
    }

    public void ResetScreen()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Black;
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            for (int j = 0; j < Console.WindowWidth; j++)
            {
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        Console.SetCursorPosition(0, 0);
    }
}
