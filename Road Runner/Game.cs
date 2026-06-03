using System.Diagnostics;
using System.Drawing;

namespace TemplateVSCode;

public enum GameState
{
    StartingScreen,
    MainMenu,
    Instructions,
    HighscoresMenu,
    NameInput,
    ColorPicker,
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
    protected Screen instructionsScreen;
    protected Screen gameOverScreen;
    protected Menu mainMenu;
    protected HighscoresScreen highScoresMenu;
    protected NameInputScreen nameInputScreen;
    protected ColorPickerScreen colorPickerScreen;
    protected StartGameMenuItem startGameMenuItem;
    protected HighscoresMenuItem highscoresMenuItem;
    protected InstructionsMenuItem instructionsMenuItem;
    protected ExitGameMenuItem exitGameMenuItem;
    protected HighscoreManager highscoreManager;
    protected bool gameStarted = false;
    protected double scrollTimer = 0;
    protected double scrollSpeed = 3;
    protected int lastSafeRow = 18;
    protected double moveTimer = 0;
    protected double moveSpeed = 0.1;
    protected bool isRespawning = false; // keeps track of whether the player is currently respawning
    protected double respawnTimer = 0; // timer for the respawn cooldown
    protected double respawnDuration = 2; // 2 seconds cooldown after respawning
    protected int score;
    protected int highestRow = 18; // keeps track of the highest row the player has reached
    protected int startX = 20; // player start x position
    protected int startY = 18; // player start y position
    protected int startLives = 3; // player start lives
    protected int roadWidth = 40; // road width
    protected bool slowmotionActive = false; // to see if slowmotion is active or not
    protected double slowmotionElapsed = 0; // how long slowmotion has been active
    protected double slowmotionDuration = 10; // slowmotion lasts 10 sec

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
        instructionsScreen = new Screen("InstructionsTxt.txt");

        mainMenu = new Menu("MainMenuTxt.txt", ConsoleColor.White, ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkRed);
        highScoresMenu = new HighscoresScreen();
        nameInputScreen = new NameInputScreen();
        colorPickerScreen = new ColorPickerScreen();

        startGameMenuItem = new StartGameMenuItem();
        highscoresMenuItem = new HighscoresMenuItem();
        instructionsMenuItem = new InstructionsMenuItem();
        exitGameMenuItem = new ExitGameMenuItem();

        mainMenu.AddMenuItem(startGameMenuItem);
        mainMenu.AddMenuItem(highscoresMenuItem);
        mainMenu.AddMenuItem(instructionsMenuItem);
        mainMenu.AddMenuItem(exitGameMenuItem);

        highscoreManager = new HighscoreManager(); // loads highscores automatically on creation
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
            case GameState.Instructions:
                if (currentGameState != previousGameState)
                {
                    ResetScreen();
                    instructionsScreen.Draw();
                }
                break;
            case GameState.HighscoresMenu:
                if (currentGameState != previousGameState)
                {
                    highScoresMenu.Draw(); // draw the title
                    highScoresMenu.Index = 0;
                    highScoresMenu.LoadHighscores(highscoreManager); // draw the list of highscores
                }
                break;
            case GameState.GameRunning:
                road.Draw(uiXOffset, uiYOffset);

                foreach (Vehicle vehicle in road.Vehicles)
                {
                    vehicle.Draw(vehicle.XPos, vehicle.YPos, vehicle.Symbol, vehicle.ForeColor, uiXOffset, uiYOffset);
                }

                foreach (Collectible collectible in road.Collectibles)
                {
                    collectible.Draw(collectible.XPos, collectible.YPos, collectible.Symbol, collectible.ForeColor, uiXOffset, uiYOffset); // draw each collectible
                }

                // temporarily change player color during respawn cooldown
                if (isRespawning)
                {
                    player.ForeColor = ConsoleColor.Gray; // grijs tijdens cooldown
                }
                else
                {
                    player.ForeColor = colorPickerScreen.SelectedColor; // normale kleur
                }

                player.Draw(player.XPos, player.YPos, player.Symbol, player.ForeColor, uiXOffset, uiYOffset);
                gameUI.Draw();
                break;

            case GameState.GameOver:
                if (currentGameState != previousGameState)
                {
                    ResetScreen();
                    gameOverScreen.Draw();
                    Console.SetCursorPosition(10, 14); // positie aanpassen naar wat er mooi uitziet
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Your score: " + score);
                }
                break;
            case GameState.NameInput:
                if (currentGameState != previousGameState)
                {
                    ResetScreen();
                    nameInputScreen.Draw();
                }
                break;
            case GameState.ColorPicker:
                if (currentGameState != previousGameState)
                {
                    ResetScreen();
                    colorPickerScreen.Draw();
                }
                break;
        }

        previousGameState = currentGameState;
    }

    public void MovePlayer(ConsoleKey key, char keyChar) //KeyChar is to be able to type a name with 'chars'
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
                    mainMenu.ActivateMenuItem(this); // pass the current game object to the menu item
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
            case GameState.Instructions:
                if (key == ConsoleKey.Backspace)
                {
                    ResetScreen();
                    currentGameState = GameState.MainMenu;
                }
                break;
            case GameState.HighscoresMenu:
                if (key == ConsoleKey.Backspace)
                {
                    ResetScreen();
                    currentGameState = GameState.MainMenu;
                }
                break;
            case GameState.GameRunning:
                if (moveTimer >= moveSpeed && !isRespawning) // only move if timer allows and player is not respawning
                {
                    player.Move(key, width, height);

                    if ((int)player.YPos < highestRow) // if player is higher than ever before
                    {
                        highestRow = (int)player.YPos; // update the highest row
                        score++; // add points
                        gameUI.UpdateUIElementValue("Score", score); // update the UI
                    }

                    int playerRowIndex = (int)player.YPos;

                    // check if the row index is within the bounds of the road
                    if (playerRowIndex >= 0 && playerRowIndex < road.Rows.Count)
                    {
                        // check if the player is standing on a grass row
                        if (road.Rows[playerRowIndex].Type == RoadElementType.Grass)
                        {
                            lastSafeRow = (int)player.YPos; // save the current screen position as the last safe position
                        }
                    }

                    gameStarted = true;
                    moveTimer = 0; // reset the move timer
                }
                break;

            case GameState.GameOver:
                if (key == ConsoleKey.Backspace)
                {
                    ResetScreen();
                    currentGameState = GameState.MainMenu;
                }
                else if (key == ConsoleKey.Enter)
                {
                    ResetGame();
                    stopwatch.Restart();
                    currentGameState = GameState.GameRunning;
                }
                break;
            case GameState.NameInput:
                if (key == ConsoleKey.Enter && nameInputScreen.PlayerName.Length > 0) //if enter is pressed and name is not empty
                {
                    ResetScreen();
                    currentGameState = GameState.ColorPicker; // go to color picker
                }
                else
                {
                    nameInputScreen.HandleInput(key, keyChar); // handle the typed character (add letter or delete letter)
                    ResetScreen();
                    nameInputScreen.Draw(); // redraw the screen with the updated name (so that player sees the new letter being added)
                }
                break;
            case GameState.ColorPicker:
                if (key == ConsoleKey.Enter) // if enter is pressed confirm the color
                {
                    player.ForeColor = colorPickerScreen.SelectedColor; // set the player color
                    ResetScreen();
                    stopwatch.Restart();
                    currentGameState = GameState.GameRunning;
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    colorPickerScreen.SelectPrevious(); // select previous color
                    ResetScreen();
                    colorPickerScreen.Draw(); // redraw with new color
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    colorPickerScreen.SelectNext(); // select next color
                    ResetScreen();
                    colorPickerScreen.Draw(); // redraw with new color
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
                    if (slowmotionActive)
                    {
                        vehicle.Update(dt * 0.5, width, height); // half the speed when slowmotion is active
                    }
                    else
                    {
                        vehicle.Update(dt, width, height);
                    }
                }
                road.UpdateCollectibles(dt, (int)player.YPos - uiYOffset); // update collectible timers and spawn new ones
                gameUI.UpdateUIElementValue("Time", (int)stopwatch.ElapsedMilliseconds / 1000);
                gameUI.UpdateUIElementValue("Lives", player.Lives);

                bool hit = CheckCollision(); // check if the player is hit by a vehicle
                CheckCollectibleCollision(); // check if player collected a collectible

                if (hit && !isRespawning) // only process hit if player is not already respawning
                {
                    if (player.ShieldActive) // if shield is active, the hit is blocked
                    {
                        player.ShieldActive = false; // shield is used up after blocking the hit
                        player.YPos = lastSafeRow; // respawn at last safe pos
                        player.XPos = startX; // respawn at center
                        isRespawning = true; // start respawn cooldown
                        respawnTimer = 0; // reset timer
                        highestRow = lastSafeRow; // reset highestRow so player can earn points again
                    }
                    else
                    {
                        player.Lives--; // subtract a life
                        player.YPos = lastSafeRow; // respawn at the last safe grass position
                        player.XPos = 20; // respawn at the center of the screen
                        isRespawning = true; // start the respawn cooldown
                        respawnTimer = 0; // reset the timer so cooldown always lasts the full duration
                        highestRow = lastSafeRow; // reset highestRow to respawn position so player can earn points again
                    }
                }

                if (isRespawning)
                {
                    respawnTimer += dt;
                    if (respawnTimer >= respawnDuration) // if cooldown is over
                    {
                        isRespawning = false; // player can move again
                        respawnTimer = 0;
                    }
                }

                if (slowmotionActive) // if slowmotion is active
                {
                    slowmotionElapsed += dt; // increment elapsed time
                    if (slowmotionElapsed >= slowmotionDuration) // if slowmotion duration is over
                    {
                        slowmotionActive = false; // deactivate slowmotion
                        slowmotionElapsed = 0; // reset timer
                    }
                }

                moveTimer += dt; // increment move timer every frame

                if (gameStarted) // only start scrolling after the player has moved
                {
                    scrollTimer += dt; // increment scroll timer

                    if (scrollTimer >= scrollSpeed) // if the timer reaches the scroll speed
                    {
                        road.Scroll(); // scroll the map

                        player.YPos++; // shift player down so they visually stay on the same row as the map scrolls
                        if (player.YPos >= height - 1)
                        {
                            player.YPos = height - 1;
                            player.Lives = 0; // player dies if pushed off screen
                        }
                        lastSafeRow++; // shift last safe row down with the scroll
                        highestRow++; // shift highest row down with the scroll

                        scrollTimer = 0; // reset the scroll timer
                    }
                }

                // increase scroll speed based on score
                if (score >= 30)
                {
                    scrollSpeed = 2; // faster after score 30
                }
                if (score >= 60)
                {
                    scrollSpeed = 1; // even faster after score 60
                }

                if (player.Lives == 0) // if the player has no lives left
                {
                    highscoreManager.AddHighscore(nameInputScreen.PlayerName, score, player.ForeColor.ToString()); // saving highscore
                    currentGameState = GameState.GameOver; // go to game over screen
                }
                break;
        }
    }

    public bool CheckCollision()
    {
        bool hit = false;
        foreach (Vehicle car in road.Vehicles)
        {
            if ((int)player.YPos == (int)car.YPos) // check if they are on the same row
            {
                if ((int)player.XPos <= (int)car.XPos + car.Symbol.Length - 1 && (int)player.XPos >= (int)car.XPos) // check if the player is within the width of the vehicle
                {
                    hit = true;
                }
            }
        }
        return hit;
    }

    public void CheckCollectibleCollision()
    {
        for (int i = road.Collectibles.Count - 1; i >= 0; i--) // loop backwards through collectibles
        {
            if ((int)player.XPos == (int)road.Collectibles[i].XPos && (int)player.YPos == (int)road.Collectibles[i].YPos) // check if player is on the same position as the collectible
            {
                switch (road.Collectibles[i].Type) // check what type of collectible it is
                {
                    case CollectibleType.Coin:
                        score += road.Collectibles[i].Points; // add points to score
                        gameUI.UpdateUIElementValue("Score", score); // update the UI
                        break;
                    case CollectibleType.Shield:
                        player.ShieldActive = true; // activate the shield
                        score += road.Collectibles[i].Points; // add points to score
                        gameUI.UpdateUIElementValue("Score", score); // update the UI
                        break;
                    case CollectibleType.Slowmotion:
                        slowmotionActive = true; // activate slowmotion
                        slowmotionElapsed = 0; // reset the timer
                        score += road.Collectibles[i].Points; // add points to score
                        gameUI.UpdateUIElementValue("Score", score); // update the UI
                        break;
                }
                road.Collectibles.RemoveAt(i); // remove collectible after collecting
            }
        }
    }

    public void ResetScreen()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Black;
        for (int i = 0; i < Console.WindowHeight + 1; i++)
        {
            for (int j = 0; j < Console.WindowWidth; j++)
            {
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        Console.SetCursorPosition(0, 0);
    }

    public void ResetGame()
    {
        ResetScreen(); // clear the screen first
        road = new Road(roadWidth);
        player.Lives = startLives;
        player.XPos = startX;
        player.YPos = startY;
        score = 0;
        gameStarted = false;
        scrollTimer = 0;
        scrollSpeed = 3; // reset scroll speed
        lastSafeRow = startY;
        highestRow = startY;
        isRespawning = false;
        slowmotionActive = false; // reset slowmotion
        slowmotionElapsed = 0;
        //nameInputScreen = new NameInputScreen();
        gameUI.UpdateUIElementValue("Score", 0);
        gameUI.UpdateUIElementValue("Lives", startLives);
        gameUI.UpdateUIElementValue("Time", 0);
    }
}