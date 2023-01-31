using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using static System.Net.Mime.MediaTypeNames;

namespace QbertCSharp
{
    /**
     * This class represents A Qbert style Game object.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    public class Qbert : Game
    {

        private SpriteBatch _spriteBatch;
        private const int MAX_DISPLAY_SCORE = 8;         // The max amount of records to display
        private const int LIFE_BONUS = 1000;             // The amount of score needed to gain a new life
        private const int TITLE_OPTIONS = 3;             // The number of menu options in the title screen

        private enum GameState { TITLE_SCREEN, GAMEPLAY, PAUSE, GAME_OVER, SCORE_TABLE, NEXT_LEVEL }     // All the different game states

        private const int NW = 0;                               // Constant for the North West Direction
        private const int NE = 1;                               // Constant for the North East Direction
        private const int SE = 2;                               // Constant for the South East Direction
        private const int SW = 3;                               // Constant for the South West Direction
        private GameState gameState;                            // The current game state

        private SpriteFont gameFont;                            // The game's font
        private Texture2D background;                           // The background texture
        private Texture2D title;                                // The title texture
        private Texture2D semiTransperent;                      // Semi transparent black texture
        private Texture2D selectionColor;                       // Selection texture texture
        private Texture2D characters;                           // The characters texture
        private GraphicsDeviceManager _graphics;                // The graphics engine
        private SpriteBatch spriteBatch;                        // The batch that paints sprites

        private QbertLevel currentLevel;                        // The current level object
        private QbertPlayer player;                             // The player
        private LinkedList<PlayerRecord> highScores;            // The list with all the high scores

        private short level;                                    // The current level number
        private int score;                                      // The current score used to keep track between levels
        private int menuSelection;                              // The element selected in the menu
        private bool buttonIsPressed;                           // A boolean that tells if the user pressed a button
        private char[] initials;                                // The latest initials used
        private int initialSelected;                            // The position of the initials selected
        private int previousLivesInrement;                      // The last time a used received a bonus
        private int lastTime;                                   // The last time the game was updated
        private int currentTime;								// The the current time the game was updated
        /**
         * The Qbert Application constructor
         * @return A new instance of the Qbert application
         * 
         * @author Dario Urdapilleta
	     * @version 1.0
	     * @since 2022-11-09
	     *	 
	     */
        public Qbert()
        {
            _graphics = new GraphicsDeviceManager(this);        // Initialize the graphics engine
            Content.RootDirectory = "Content";                  // The content Root directory
            IsMouseVisible = false;                             // Hide the mouse
        }
        /**
	     * Initializes the game. This method is called once when the application starts.
	     * 
	     * @author Dario Urdapilleta
	     * @version 1.0
	     * @since 2022-11-09
	     *	 
	     */
        protected override void Initialize()
        {
            this.previousLivesInrement = 0;                                                             // Sets the previous lives increment to 0
            this.initials = new char[PlayerRecord.NAME_SIZE];                                           // Instantiates the initials
            for (int initialsCounter = 0; initialsCounter < this.initials.Length; initialsCounter++)    // Loops through the initials
            {
                this.initials[initialsCounter] = 'A';                                                   // Sets each initial with a starting value of A
            }
            this.LoadScores();                                                                          // Loads the scores from the file
            this.menuSelection = 0;                                                                     // Sets the initial value of the menu selection to 0
            this.buttonIsPressed = false;                                                               // Sets the initial value of a button pressed to false
            this.gameState = GameState.TITLE_SCREEN;                                                    // Sets the initial game state to TITLE_SCREEN
		    this.lastTime = DateTime.Now.Millisecond;                                                   // Set the lst time
            this.SetFullscreen();                                                                       // Sets the game to fulls creen mode
            base.Initialize();
        }
        /**
	     * Loads the content
	     * 
	     * @author Dario Urdapilleta
	     * @version 1.0
	     * @since 2022-11-09
	     *	 
	     */
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);                                          // Instantiates the Sprite Batch					    
            this.background = Content.Load<Texture2D>("Qbert");                                     // Loads the background texture
            this.title = Content.Load<Texture2D>("Title");                                          // Loads the title texture
            this.characters = Content.Load<Texture2D>("AnimationSpritelist");                   // Loads the characters texture
            this.semiTransperent = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color); // Create the semi transparent texture
            Color[] transparentBlackColor = new Color[1];                                           // Create a color array
            transparentBlackColor[0] = Color.FromNonPremultiplied(new Vector4(0, 0, 0, 0.7f));      // Set the color
            this.semiTransperent.SetData<Color>(transparentBlackColor);                             // Set the color array to the texture
            this.selectionColor = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);  // Create the semi transparent texture
            Color[] selectionColorArray = new Color[1];                                             // Create a color array
            selectionColorArray[0] = Color.FromNonPremultiplied(new Vector4(120, 100, 30, 0.8f));   // Set the color
            this.selectionColor.SetData<Color>(selectionColorArray);                                // Set the color array to the texture
            this.gameFont = Content.Load<SpriteFont>("File");    						// Sets the game font
        }
        /**
	     * Updates the logic of the game.
	     * @param gameTime The time passed since last update.
	     * 
	     * @author Dario Urdapilleta
	     * @version 1.0
	     * @since 2022-11-09
	     *	 
	     */
        protected override void Update(GameTime gameTime)
        {
            this.input();											// Get the input from the player
            switch (this.gameState)                                                                             // Do different things depending on the game state
            {
                case GameState.GAMEPLAY:                                                                        // When the game state is GAMEPLAY
                    this.currentLevel.Update(gameTime);                                                         // Update the current level
                    if (this.currentLevel.GetCurrentScore() - this.previousLivesInrement > Qbert.LIFE_BONUS)    // If the player's score has gone above the life bonus
                    {
                        this.player.OneUp();                                                                    // Add one life to the player 
                        this.previousLivesInrement = this.currentLevel.GetCurrentScore();                       // Update the next life increment step
                    }
                    if (this.currentLevel.GameCompleted())                                                      // If the game is completed
                    {
                        this.gameState = GameState.NEXT_LEVEL;                                                  // Change the state to the NEXT_LEVEL
                    }
                    if (this.player.IsDead())                                                                   // if the player is dead
                    {
                        this.initialSelected = 0;                                                               // Set the initial selected to the first letter
                        this.gameState = GameState.GAME_OVER;                                                   // Change the game state to the GAME_OVER state
                    }
                    break;
                default:                                                                                        // The rest of the states don't need to do anything
                    break;
            }

            base.Update(gameTime);
        }
        /**
	     * Handles the keyboard input.
	     * 
	     * @author Dario Urdapilleta
	     * @version 1.0
	     * @since 2022-11-09
	     *	 
	     */
        private void input()
        {
            switch (this.gameState)                                         // Handle keys differently depending on the game state
            {                                                           
                case GameState.TITLE_SCREEN:                                // For the TITLE_SCREEN
                    if (!this.buttonIsPressed)                              // Make sure no button is pressed
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))         // Handle if the UP key is pressed
                        {                                       
                            if (this.menuSelection > 0)                     // Make sure the selected element is greater than 0
                            {                                           
                                this.buttonIsPressed = true;                // Let the class know a button was pressed
                                this.menuSelection--;                       // Reduce the menu selection by 1
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Down))  // Handle the DOWN key
                        {                               
                            if (this.menuSelection < TITLE_OPTIONS - 1)     // Make sure the current selected item can go up at least once
                            {                            
                                this.buttonIsPressed = true;                // Let the class know a button was pressed	
                                this.menuSelection++;                       // Increment the menu selection
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter)) // Handle pressing the ENTER key
                        {                           
                            this.buttonIsPressed = true;                    // Let the class know a button was pressed	
                            switch (this.menuSelection)                     // Do something different depending on the selected option
                            {           
                                case 0:                                     // Start game was selected
                                    this.level = 1;                         // Set the initial level to 1
                                    this.score = 0;                         // Set the initial score to 0
                                    this.player = new QbertPlayer(this.characters);        // Create a new Player
                                    this.LoadLevel();                       // Load a new level
                                    this.gameState = GameState.GAMEPLAY;    // Change the game state to GAMEPLAY
                                    break;
                                case 1:                                     // High score option was selected
                                    this.gameState = GameState.SCORE_TABLE; // Change the game state to SCORE_TABLE
                                    break;
                                case 2:                                     // Quit was selected
                                    Exit();                                 // Exit the game
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                case GameState.SCORE_TABLE:                                 // Handle input in the Score Table
                    if (!this.buttonIsPressed)                              // Make sure no button is pressed
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))      // Handle pressing the ENTER key
                        {
                            this.gameState = GameState.TITLE_SCREEN;        // Return to the TITLE_SCREEN
                            this.buttonIsPressed = true;                    // Let the class know a button was pressed	
                        }
                    }
                    break;
                case GameState.GAME_OVER:                                   // Handle input in the GAME_OVER screen
                    if (!this.buttonIsPressed)                              // Make sure no button is pressed
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))      // Handle pressing the ENTER key
                        {
                            this.SaveScore(this.initials, this.currentLevel.GetCurrentScore());     // Save the current score in the score list and file
                            this.gameState = GameState.TITLE_SCREEN;                                // Change the game state to the TITLE_SCREEN
                            this.buttonIsPressed = true;                                            // Let the class know a button was pressed
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Left))  // Handle pressing the LEFT key
                        {
                            if (this.initialSelected > 0)                   // Make sure the initial selected is not the first one
                            {
                                this.buttonIsPressed = true;                // Let the class know a button was pressed
                                this.initialSelected--;                     // Reduce the selected initial by 1
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Right)) // Handle pressing the RIGHT button
                        {
                            if (this.initialSelected < PlayerRecord.NAME_SIZE - 1)                  // Make sure the letter selected is not the last one
                            {
                                this.buttonIsPressed = true;                                        // Let the class know a button was pressed
                                this.initialSelected++;                                             // Increment the initial selected
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Up))    // Handle the UP key
                        {
                            if (this.initials[initialSelected] == 'Z')      // Check if the current letter is Z or more
                            {                               
                                this.initials[initialSelected] = 'A';       // Change it to A
                            }
                            else
                            {
                                this.initials[initialSelected]++;           // Otherwise, increment it by 1
                            }
                            this.buttonIsPressed = true;                    // Let the class know a button was pressed
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Down))  // Handle the DOWN key
                        {
                            if (this.initials[initialSelected] <= 'A')      // Check if the current is A or lower
                            {
                                this.initials[initialSelected] = 'Z';       // Set it to Z
                            }
                            else
                            {
                                this.initials[initialSelected]--;           // Decrease the letter by 1
                            }
                            this.buttonIsPressed = true;                    // Let the class know a button was pressed
                        }
                    }
                    break;
                case GameState.GAMEPLAY:                                    // Handle input in the GAMEPLAY screen
                    if (!this.buttonIsPressed)                              // Make sure no button is pressed
                    {
                        if (!this.player.IsMoving())                        // Make sure the player is not moving
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Q))      // Handle pressing Q (North West)
                            {
                                currentLevel.MovePlayer(NW);                // Move the player North West
                            }
                            else if (Keyboard.GetState().IsKeyDown(Keys.W)) // Handle pressing W (North East)
                            {                           
                                currentLevel.MovePlayer(NE);                // Move the player North East
                            }
                            else if (Keyboard.GetState().IsKeyDown(Keys.S)) // Handle pressing S (South East)
                            {
                                currentLevel.MovePlayer(SE);                // Move the player South East
                            }
                            else if (Keyboard.GetState().IsKeyDown(Keys.A)) // Handle pressing A (South West)
                            {                           
                                currentLevel.MovePlayer(SW);                // Move the player South West
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.P))          // Handle pressing the P key
                        {
                            this.gameState = GameState.PAUSE;               // Change the game state to PAUSE
                            this.buttonIsPressed = true;                    // Let the class know a button was pressed
                        }
                    }
                    break;
                case GameState.NEXT_LEVEL:                                  // Handle the input in the NEXT_LEVEL screen
                    if (!this.buttonIsPressed)                              // Make sure no button is pressed
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))      // Handle pressing the ENTER key
                        {
                            this.score = this.currentLevel.GetCurrentScore();   // Update the score with the previous level score
                            this.level++;                                       // Increment the level
                            this.LoadLevel();                                   // Load a new Level
                            this.gameState = GameState.GAMEPLAY;                // Change the game state to GAMEPLAY
                        }
                    }
                    break;
                case GameState.PAUSE:                                       // Handle when the input on the PAUSE screen
                    if (!this.buttonIsPressed)                              // Make sure no button is pressed
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.P))          // Handle pressing the P key
                        {
                            this.gameState = GameState.GAMEPLAY;            // Change back to GAMEPLAY state
                            this.buttonIsPressed = true;                    // Let the class know a button was pressed
                        }
                    }
                    break;
                default:                                                    // No other Game states
                    break;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))                 // Check if the ESCAPE buuton is pressed
            {
                Exit();                                                     // Exit the game
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.P)                      // Make sure no important key is pressed
                    && !Keyboard.GetState().IsKeyDown(Keys.Up)
                    && !Keyboard.GetState().IsKeyDown(Keys.Down)
                    && !Keyboard.GetState().IsKeyDown(Keys.Right)
                    && !Keyboard.GetState().IsKeyDown(Keys.Left)
                    && !Keyboard.GetState().IsKeyDown(Keys.Enter))
            {                                   
                this.buttonIsPressed = false;                                // Let the class know no button is pressed
            }
        }
        /**
	     * Draws for the camera.
	     * 
	     * @author Dario Urdapilleta
	     * @version 1.0
	     * @since 2022-11-09
	     *	 
	     */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);                                             // Clear the screen
            spriteBatch.Begin();                                                                    // Begin the sprite batch
            spriteBatch.Draw(this.background, new Rectangle(0, 0, 1920, 1080), Color.White);        // Render the Background for all states
            switch (this.gameState)                                                                 // Paint differently depending on the game state
            {
                case GameState.TITLE_SCREEN:                                                        // For the TITLE_SCREEN
                    spriteBatch.Draw(this.semiTransperent, new Rectangle(750, 560, 405, 290), Color.White);                                         // Render a rectangle for the background
                    spriteBatch.Draw(this.selectionColor, new Rectangle(750, 560 + this.menuSelection * 100, 405, 90), new Color(120, 100, 30));    // Render a rectangle for the selection
                    spriteBatch.Draw(this.title, new Rectangle(420, 200, 1120, 226), Color.White);  // Render the title screen
                    spriteBatch.DrawString(this.gameFont,                                           // Render the first menu option
                        "New Game", 
                        new Vector2(960, 600),
                        Color.White, 0, 
                        this.gameFont.MeasureString("New Game") / 2, 
                        1.0f, 
                        SpriteEffects.None, 
                        0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the second menu option
                        "High Score",
                        new Vector2(955, 700),
                        Color.White, 0,
                        this.gameFont.MeasureString("High Score") / 2,
                        1.0f,
                        SpriteEffects.None,
                        0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the third menu option
                        "Quit Game",
                        new Vector2(950, 800),
                        Color.White, 0,
                        this.gameFont.MeasureString("Quit Game") / 2,
                        1.0f,
                        SpriteEffects.None,
                        0.5f);
                    break;
                case GameState.SCORE_TABLE:                                                         // Draw for the SCORE TABLE
                    spriteBatch.Draw(this.semiTransperent, new Rectangle(0, 0, 1960, 1080), Color.White);                                 // Render a rectangle for the background
                    spriteBatch.DrawString(this.gameFont,                                           // Render the title
                        "HIGH SCORES",
                        new Vector2(980, 100),
                        Color.White, 0,
                        this.gameFont.MeasureString("HIGH SCORES") / 2,
                        1.0f,
                        SpriteEffects.None,
                        0.5f);
                    int recordsDisplayed = 0;                                                           // Variable to count the records displayed
                    for (LinkedListNode<PlayerRecord> scoreCounter = this.highScores.First; scoreCounter != null && recordsDisplayed < MAX_DISPLAY_SCORE; scoreCounter = scoreCounter.Next) // Loop through the highscores but don't loop more then the max scores
                    {
                        spriteBatch.DrawString(this.gameFont,                                           // Draw the name
                        scoreCounter.Value.GetStringName() + " " + scoreCounter.Value.GetScore().ToString(),
                        new Vector2(980, 200 + (recordsDisplayed * 100)),
                        Color.White, 0,
                        this.gameFont.MeasureString(scoreCounter.Value.GetStringName() + " " + scoreCounter.Value.GetScore().ToString()) / 2,
                        1.0f,
                        SpriteEffects.None,
                        0.5f);
                        recordsDisplayed++;                                                             // Increase the record displayed
                    }
                    spriteBatch.DrawString(this.gameFont,                                               // Draw the instructions
                    "Press ENTER to return to the Title Screen.",
                    new Vector2(980, 980),
                    Color.White, 0,
                    this.gameFont.MeasureString("Press ENTER to return to the Title Screen.") / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    break;
                case GameState.NEXT_LEVEL:                                                          // Draw the NEXT_LEVEL
                    spriteBatch.Draw(this.semiTransperent, new Rectangle(510, 370, 910, 310), Color.White);     // Render a rectangle for the background
                    spriteBatch.DrawString(this.gameFont,                                           // Render the completed message
                    "LEVEL " + this.level + " COMPLETED!",
                    new Vector2(980, 450),
                    Color.White, 0,
                    this.gameFont.MeasureString("LEVEL " + this.level + " COMPLETED!") / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the instructions
                    "Press ENTER to continue.",
                    new Vector2(980, 610),
                    Color.White, 0,
                    this.gameFont.MeasureString("Press ENTER to continue.") / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    break;
                case GameState.GAME_OVER:                                                           // Draw the GAME_OVER
                    spriteBatch.Draw(this.semiTransperent, new Rectangle(380, 300, 1170, 510), Color.White);                            // Render a rectangle for the background
                    spriteBatch.Draw(this.selectionColor, new Rectangle(1202 + this.initialSelected * 36, 570, 30, 60), new Color(120, 100, 30));   // Render a rectangle for the selection
                    spriteBatch.DrawString(this.gameFont,                                           // Render the title
                    "GAME OVER!",
                    new Vector2(980, 400),
                    Color.White, 0,
                    this.gameFont.MeasureString("GAME OVER!") / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the score
                    "Your score was: " + this.currentLevel.GetCurrentScore(),
                    new Vector2(980, 500),
                    Color.White, 0,
                    this.gameFont.MeasureString("Your score was: " + this.currentLevel.GetCurrentScore()) / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the initials
                    "Your initials: " + this.initials[0].ToString() + this.initials[1].ToString() + this.initials[2].ToString(),
                    new Vector2(980, 600),
                    Color.White, 0,
                    this.gameFont.MeasureString("Your initials: " + this.initials[0].ToString() + this.initials[1].ToString() + this.initials[2].ToString()) / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // render the instructions
                    "Press ENTER to save your score.",
                    new Vector2(980, 700),
                    Color.White, 0,
                    this.gameFont.MeasureString("Press ENTER to save your score.") / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    break;
                case GameState.PAUSE:                                                               // Draw the PAUSE
                    spriteBatch.Draw(this.semiTransperent, new Rectangle(0, 0, 1960, 1080), Color.White);       // Render a rectangle for the background
                    spriteBatch.DrawString(this.gameFont,                                           // Render the title
                    "PAUSED",
                    new Vector2(980, 580),
                    Color.White, 0,
                    this.gameFont.MeasureString("PAUSED") / 2,
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    break;
                case GameState.GAMEPLAY:                                                            // Draw the GAMEPLAY
                    spriteBatch.Draw(this.semiTransperent, new Rectangle(20, 30, 700, 210), Color.White);   // Render a rectangle for the background
                    this.currentLevel.DrawPlayer(spriteBatch, true);                                // Render the player only if it is falling behind
                    this.currentLevel.DrawEnemies(spriteBatch, true);                               // Render the enemies falling behind
                    spriteBatch.End();                                                              // Finish the sprite batch
                    this.currentLevel.DrawBlocks(GraphicsDevice);                                   // Draw the platforms
                    spriteBatch.Begin();                                                            // Start the sprite batch
                    this.currentLevel.DrawPlayer(spriteBatch, false);                               // Render the player if it is in fron of the platforms
                    this.currentLevel.DrawEnemies(spriteBatch, false);                              // Render the enemies that are in fron of the platforms
                    spriteBatch.DrawString(this.gameFont,                                           // Render the current score
                    "Score: " + (this.currentLevel.GetCurrentScore()),
                    new Vector2(50, 30),
                    Color.White, 0,
                    new Vector2(0,0),
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the current level
                    "Level: " + this.level,
                    new Vector2(50, 90),
                    Color.White, 0,
                    new Vector2(0, 0),
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    spriteBatch.DrawString(this.gameFont,                                           // Render the current lives
                    "Lives: " + this.currentLevel.playersLives(),
                    new Vector2(50, 150),
                    Color.White, 0,
                    new Vector2(0, 0),
                    1.0f,
                    SpriteEffects.None,
                    0.5f);
                    break;
                default:                                                                            // No more states to handle
                    break;
            }
            spriteBatch.End();                                                                      // End the sprite batch
            base.Draw(gameTime);                                                                    // Call the parents Draw method
        }
        /**
         * Loads a level depending on the current level number.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private void LoadLevel()
        {
            currentLevel = new QbertLevel(this.level, ref this.player, this.score, GraphicsDevice, this.characters);         // Create a new level
        }
        /**
         * Loads the scores from a binary file.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private void LoadScores()
        {
            this.highScores = new LinkedList<PlayerRecord>();                           // Create a new empty high score list
            int score = -1, nameCounter, scoreCounter, bytesCounter;                    // Declare all the variables that will be used that are integers
            byte[] nameInBytes, scoreIntBytes, allBytes;                                // Declare all the byte arrays that will be used
            char[] name;                                                                // Declare the char array for the name
            try
            {
                allBytes = File.ReadAllBytes("scores.dat");                             // Create a byte array to store all the bytes
                nameInBytes = new byte[PlayerRecord.NAME_SIZE];                         // Create a byte array to store the name
                scoreIntBytes = new byte[4];                                            // Create a byte array to store the score
                name = new char[PlayerRecord.NAME_SIZE];                                // Create the char array for the name
                for (bytesCounter = 0; bytesCounter < allBytes.Length; bytesCounter += 7)       // Loop for each record
                {       
                    for (nameCounter = 0; nameCounter < PlayerRecord.NAME_SIZE; nameCounter++)  // Loop three times, one for each char of the name
                    {
                        name[nameCounter] = (char)allBytes[bytesCounter + nameCounter];         // Store the char into the name byte array
                    }
                    for (scoreCounter = 0; scoreCounter < 4; scoreCounter++)            // Loop four times for each byte that is part of the score
                    {           
                        scoreIntBytes[scoreCounter] = allBytes[bytesCounter + PlayerRecord.NAME_SIZE + scoreCounter];       // Store the byte into the score byte array
                    }
                    score = BitConverter.ToInt32(scoreIntBytes, 0);                             // Convert the information to an int
                    this.highScores.AddLast(new PlayerRecord(name, score));             // Add the read score
                }
            }
            catch (IndexOutOfRangeException exception)                                  // Handle When an index is out of range
            {
                Console.WriteLine("Index out of range: " + exception.Message);      // Print in console the message
            }
            catch (UnauthorizedAccessException exception)                               // Handle there's no access
            {
                Console.WriteLine("Can't access this file: " + exception.Message);      // Print in console the message
            }
            catch (IOException exception)                                               // Handle any other IO exception
            {
                Console.WriteLine("Error Opening the file: " + exception.Message);      // Print in console the message
            }
        }
        /**
         * Adds a new score into the list and saves the scores into a file.
         * @param name The new score initials.
         * @param score The new score.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private void SaveScore(char[] name, int score)
        {
            this.AddScore(new PlayerRecord(name, score));                           // Add the score to the list
            try
            {
                int scoresRecorded = 0;                                                 // Start the recorder counter
                int nameCounter;                                                        // declare the name counter
                File.Delete("scores.dat");                                              // Delete the current file
                FileStream stream = File.Open("scores.dat", FileMode.Create);           // Create the new file
                BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false);   // Create the file writer
                for (LinkedListNode<PlayerRecord> scoreCounter = this.highScores.First; scoreCounter != null && scoresRecorded < MAX_DISPLAY_SCORE; scoreCounter = scoreCounter.Next) // Loop through the highscores but don't loop more then the max scores
                {
                    for(nameCounter = 0; nameCounter < PlayerRecord.NAME_SIZE; nameCounter++)
                    {
                        writer.Write(scoreCounter.Value.GetStringName()[nameCounter]);        // Write the initial's letter
                    }
                    writer.Write(scoreCounter.Value.GetScore());                        // Write the score
                }
                stream.Close();                                                         // Close the file
            }
            catch (IndexOutOfRangeException exception)                                  // Handle the exception in case there's an index out of bounds in an array
            {
                Console.WriteLine("Index out of range: " + exception.Message);          // Print in console the message
            }
            catch (UnauthorizedAccessException exception)                               // Handle there's no access
            {
                Console.WriteLine("Can't access this file: " + exception.Message);      // Print in console the message
            }
            catch (IOException exception)                                               // Handle any other IO exception
            {
                Console.WriteLine("Error Opening the file: " + exception.Message);      // Print in console the message
            }
        }
        /**
         * Adds a new score into the list.
         * @param score The new PlayerScore
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private void AddScore(PlayerRecord score)
        {
            bool foundRank = false;                                                                             // Declare a false boolean to see if a location for the new score has been found
            int rank = 0;                                                                                       // Declare an int for the new score's position
            PlayerRecord smaller = null;                                                                        // Declare the element to add it before
            for (LinkedListNode<PlayerRecord> scoreCounter = this.highScores.First; scoreCounter != null && !foundRank; scoreCounter = scoreCounter.Next)      // Loop through the score list until it is over or an adequate rank has been found
            {       
                if (scoreCounter.Value.GetScore() < score.GetScore())                                           // Check if the new score is higher than the current score
                {                           
                    foundRank = true;                                                                           // We have found an appropriate rank
                    this.highScores.AddBefore(scoreCounter, score);                                             // Add the new score before th
                }
                else
                {
                    rank++;                                                                                     // Otherwise increment the position
                }
            }
            if(!foundRank)                                                                                      // If no score lower was found
            {
                this.highScores.AddLast(score);                                                                 // Add the score at the end
            }
        }
        /**
         * Configures the full-screen modes
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private void SetFullscreen()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;       // Sets the preferred width
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;     // Sets the preferred height
            _graphics.HardwareModeSwitch = true;                                                                // Enables the hardware mode switch
            _graphics.IsFullScreen = true;                                                                      // Sets full screen mode
            _graphics.ApplyChanges();                                                                           //´Applies the changes
        }
    }
}
