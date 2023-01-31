package com.urdapilleta.qbert;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.ByteBuffer;
import java.nio.IntBuffer;
import java.util.LinkedList;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.PolygonSpriteBatch;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;

/**
 * This class represents A Qbert style Game object.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class Qbert extends ApplicationAdapter {
	private static final int MAX_DISPLAY_SCORE = 8; 		// The max amount of records to display
	private static final int LIFE_BONUS = 1000;				// The amount of score needed to gain a new life
	private static final int TITLE_OPTIONS = 3;				// The number of menu options in the title screen
	
	private static enum GameState {TITLE_SCREEN, GAMEPLAY, PAUSE, GAME_OVER, SCORE_TABLE, NEXT_LEVEL}		// All the different game states
	
	private final int NW = 0;								// Constant for the North West Direction
	private final int NE = 1;								// Constant for the North East Direction
	private final int SE = 2;								// Constant for the South East Direction
	private final int SW = 3;								// Constant for the South West Direction
	private GameState gameState;							// The current game state
	
	private OrthographicCamera camera;						// The game's camera
	private BitmapFont gameFont;							// The game's font
	private Texture background;								// The background texture
	private Texture title;									// The title texture
	private SpriteBatch spriteBatch;						// The batch that paints sprites
	private PolygonSpriteBatch polyBatch;					// The batch that paints polygons for the platforms
	private ShapeRenderer shapeRenderer;					// The shape renderer
	
	private QbertLevel currentLevel;						// The current level object
	private QbertPlayer player;								// The player
	private LinkedList<PlayerRecord> highScores;			// The list with all the high scores
	
	private short level;									// The current level number
	private int score;										// The current score used to keep track between levels
	private int menuSelection;								// The element selected in the menu
	private boolean buttonIsPressed;						// A boolean that tells if the user pressed a button
	private char[] initials;								// The latest initials used
	private int initialSelected;							// The position of the initials selected
	private int previousLivesInrement;						// The last time a used received a bonus
	private float lastTime;									// The last time the game was updated
	private float currentTime;								// The the current time the game was updated
	/**
	 * Initializes the game. This method is called once when the application starts.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	@Override
	public void create () {
		this.previousLivesInrement = 0;																// Sets the previous lives increment to 0
		this.initials = new char[PlayerRecord.NAME_SIZE];											// Instantiates the initials
		for(int initialsCounter = 0; initialsCounter < this.initials.length;initialsCounter++) {	// Loops through the initials
			this.initials[initialsCounter] = 'A';													// Sets each initial with a starting value of A
		}
		this.loadScores();																			// Loads the scores from the file
		this.menuSelection = 0;																		// Sets the initial value of the menu selection to 0
		this.buttonIsPressed = false;																// Sets the initial value of a button pressed to false
		this.gameState = GameState.TITLE_SCREEN;													// Sets the initial game state to TITLE_SCREEN
		this.gameFont = new BitmapFont(Gdx.files.internal("syne.fnt"), false);						// Sets the game font
		this.camera = new OrthographicCamera();														// Instantiates a new Camera
		this.camera.setToOrtho(false, Gdx.graphics.getWidth(), Gdx.graphics.getHeight());			// Sets the camera to orthogonal view
		this.spriteBatch = new SpriteBatch();														// Instantiates the Sprite Batch
		this.polyBatch = new PolygonSpriteBatch();													// Instantiates the polygon Batch
		this.shapeRenderer = new ShapeRenderer();													// Instantiates the Shape Renderer
		this.background = new Texture(Gdx.files.internal("Qbert.png"));								// Loads the background texture
		this.title = new Texture(Gdx.files.internal("Title.png"));									// Loads the title texture
		this.lastTime = System.nanoTime();															// Sets the initial time
	}
	/**
	 * Deallocates the graphic memory.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	@Override
	public void dispose () {
		this.background.dispose();			// Deallocate the background texture
		this.title.dispose();				// Deallocate the title texture
		this.polyBatch.dispose();			// Deallocate the polygon batch
		this.spriteBatch.dispose();			// Deallocate the sprite batch
		this.shapeRenderer.dispose();		// deallocate the Shape Renderer
		this.player.dispose();				// Deallocate the player's sprites
		if(this.currentLevel != null) {		// Ensures there's a level loaded
			this.currentLevel.dispose();	// Deallocate the current level
		}
	}
	/**
	 * Handles the games main loop
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	@Override
	public void render () {
		this.currentTime = System.nanoTime()/10000000;			// Get the current time and modulate it to a manageable number
		float timeSlice = this.currentTime - this.lastTime;		// Calculate the time passed since last update
		this.lastTime = currentTime;							// Updates the last time to the current time
		this.input();											// Get the input from the player
		this.update(timeSlice);									// Update
		this.draw();											// Draw the screen
	}
	/**
	 * Updates the logic of the game.
	 * @param timeslice The time passed since last update.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	private void update(float timeslice) {
		camera.update();																				// Update the camera
		switch(this.gameState) {																		// Do different things depending on the game state
		case GAMEPLAY:																					// When the game state is GAMEPLAY
			this.currentLevel.update(timeslice);														// Update the current level
			if(this.currentLevel.getCurrentScore() - this.previousLivesInrement > Qbert.LIFE_BONUS) {	// If the player's score has gone above the life bonus
				this.player.oneUp();																	// Add one life to the player 
				this.previousLivesInrement = this.currentLevel.getCurrentScore();						// Update the next life increment step
			}
			if(this.currentLevel.gameCompleted()) {														// If the game is completed
				this.gameState = GameState.NEXT_LEVEL;													// Change the state to the NEXT_LEVEL
			}
			if(this.player.isDead()) {																	// if the player is dead
				this.initialSelected = 0;																// Set the initial selected to the first letter
				this.gameState = GameState.GAME_OVER;													// Change the game state to the GAME_OVER state
			}
			break;
		default:																						// The rest of the states don't need to do anything
			break;
		}
	}
	/**
	 * Handles the keyboard input.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	private void input() {
		switch(this.gameState) {															// Handle keys differently depending on the game state
		case TITLE_SCREEN:																	// For the TITLE_SCREEN
			if(!this.buttonIsPressed) {														// Make sure no button is pressed
				if(Gdx.input.isKeyPressed(Keys.UP)) {										// Handle if the UP key is pressed
					if(this.menuSelection > 0) {											// Make sure the selected element is greater than 0
						this.buttonIsPressed = true;										// Let the class know a button was pressed				
						this.menuSelection--;												// Reduce the menu selection by 1
					}
				} else if (Gdx.input.isKeyPressed(Keys.DOWN)) {								// Handle the DOWN key
					if(this.menuSelection < TITLE_OPTIONS - 1) {							// Make sure the current selected item can go up at least once 
						this.buttonIsPressed = true;										// Let the class know a button was pressed	
						this.menuSelection++;												// Increment the menu selection
					}
				} else if (Gdx.input.isKeyPressed(Keys.ENTER)) {							// Handle pressing the ENTER key
					this.buttonIsPressed = true;											// Let the class know a button was pressed	
					switch(this.menuSelection) {											// Do something different depending on the selected option
					case 0:																	// Start game was selected
						this.level = 1;														// Set the initial level to 1
						this.score = 0;														// Set the initial score to 0
						if(this.player != null) {											// Make sure the player is created before clearing the memory
							this.player.dispose();											// Clean the player's memory
						}
						this.player = new QbertPlayer(); 									// Create a new Player
						this.loadLevel();													// Load a new level
						this.gameState = GameState.GAMEPLAY;								// Change the game state to GAMEPLY
						break;
					case 1:																	// High score option was selected
						this.gameState =GameState.SCORE_TABLE;								// Change the game state to SCORE_TABLE
						break;
					case 2:																	// Quit was selected
						Gdx.app.exit();														// Quit the game
						break;
					default:
						break;
					}
				} 
			}
			break;
		case SCORE_TABLE:																	// Handle input in the Score Table
			if(!this.buttonIsPressed) {														// Make sure no button is pressed
				if (Gdx.input.isKeyPressed(Keys.ENTER)) {									// Handle pressing the ENTER key
					this.gameState = GameState.TITLE_SCREEN;								// Return to the TITLE_SCREEN
					this.buttonIsPressed = true;											// Let the class know a button was pressed	
				}
			}
			break;
		case GAME_OVER:																		// Handle input in the GAME_OVER screen
			if(!this.buttonIsPressed) {														// Make sure no button is pressed
				if (Gdx.input.isKeyPressed(Keys.ENTER)) {									// Handle pressing the ENTER key
					this.saveScore(this.initials, this.currentLevel.getCurrentScore());		// Save the current score in the score list and file
					this.gameState = GameState.TITLE_SCREEN;								// Change the game state to the TITLE_SCREEN
					this.buttonIsPressed = true;											// Let the class know a button was pressed
				} else if(Gdx.input.isKeyPressed(Keys.LEFT)) {								// Handle pressing the LEFT key
					if(this.initialSelected > 0) {											// Make sure the initial selected is not the first one
						this.buttonIsPressed = true;										// Let the class know a button was pressed
						this.initialSelected--;												// Reduce the selected initial by 1
					}
				} else if (Gdx.input.isKeyPressed(Keys.RIGHT)) {							// Handle pressing the RIGHT button
					if(this.initialSelected < PlayerRecord.NAME_SIZE - 1) {					// Make sure the letter selected is not the last one
						this.buttonIsPressed = true;										// Let the class know a button was pressed
						this.initialSelected++;												// Increment the initial selected
					}
				} else if (Gdx.input.isKeyPressed(Keys.UP)) {								// Handle the UP key
					if(this.initials[initialSelected] >= 90) {								// Check if the current letter is Z or more
						this.initials[initialSelected] = 65;								// Change it to A
					} else {
						this.initials[initialSelected]++;									// Otherwise, increment it by 1
					}
					this.buttonIsPressed = true;											// Let the class know a button was pressed
				} else if (Gdx.input.isKeyPressed(Keys.DOWN)) {								// Handle the DOWN key
					if(this.initials[initialSelected] <= 65) {								// Check if the current is A or lower
						this.initials[initialSelected] = 90;								// Set it to Z
					} else {
						this.initials[initialSelected]--;									// Decrease the letter by 1
					}
					this.buttonIsPressed = true;											// Let the class know a button was pressed
				}
			}
			break;
		case GAMEPLAY:																		// Handle input in the GAMEPLAY screen
			if(!this.buttonIsPressed) {														// Make sure no button is pressed
				if(!this.player.isMoving()) {												// Make sure the player is not moving
					if(Gdx.input.isKeyPressed(Keys.Q)) {									// Handle pressing Q (North West)
						currentLevel.movePlayer(NW);										// Move the player North West
					} else if (Gdx.input.isKeyPressed(Keys.W)) {							// Handle pressing W (North East)
						currentLevel.movePlayer(NE);										// Move the player North East
					} else if (Gdx.input.isKeyPressed(Keys.S)) {							// Handle pressing S (South East)
						currentLevel.movePlayer(SE);										// Move the player South East
					} else if (Gdx.input.isKeyPressed(Keys.A)) {							// Handle pressing A (South West)
						currentLevel.movePlayer(SW);										// Move the player South West
					}
				}
				if(Gdx.input.isKeyPressed(Keys.P)) {										// Handle pressing the P key
					this.gameState = GameState.PAUSE;										// Change the game state to PAUSE
					this.buttonIsPressed = true;											// Let the class know a button was pressed
				}
			}
			break;
		case NEXT_LEVEL:																	// Handle the input in the NEXT_LEVEL screen
			if(!this.buttonIsPressed) {														// Make sure no button is pressed
				if (Gdx.input.isKeyPressed(Keys.ENTER)) {									// Handle pressing the ENTER key
					this.score = this.currentLevel.getCurrentScore();						// Update the score with the previous level score
					this.level++;															// Increment the level
					this.loadLevel();														// Load a new Level
					this.gameState = GameState.GAMEPLAY;									// Change the game state to GAMEPLAY
				}
			}
			break;
		case PAUSE:																			// Handle when the button is PRESSED
			if(!this.buttonIsPressed) {														// Make sure no button is pressed
				if(Gdx.input.isKeyPressed(Keys.P)) {										// Handle pressing the P key
					this.gameState = GameState.GAMEPLAY;									// Change back to GAMEPLAY state
					this.buttonIsPressed = true;											// Let the class know a button was pressed
				}
			}
			break;
		default:																			// No other Game states
			break;
		}
		if(Gdx.input.isKeyPressed(Keys.ESCAPE)) {											// Handle pressing the ESCAPE key for all game states
			Gdx.app.exit();																	// Exit the application
		}
		if(!Gdx.input.isKeyPressed(Keys.P) 
				&& !Gdx.input.isKeyPressed(Keys.UP) 
				&& !Gdx.input.isKeyPressed(Keys.DOWN) 
				&& !Gdx.input.isKeyPressed(Keys.RIGHT) 
				&& !Gdx.input.isKeyPressed(Keys.LEFT) 
				&& !Gdx.input.isKeyPressed(Keys.ENTER)){									// Make sure no important key is pressed
			this.buttonIsPressed = false;													// Let the class know no button is pressed
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
	private void draw() {
		Gdx.gl.glClearColor(1,1,1,1);														// Set the clear color
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);											// Clear the screen
		spriteBatch.begin();																// Begin the sprite batch
		spriteBatch.draw(background,0,0);													// Render the Background for all states
		spriteBatch.end();																	// End the sprite batch
		switch(this.gameState) {															// Paint differently depending on the game state
		case TITLE_SCREEN:																	// For the TITLE_SCREEN
			Gdx.gl.glEnable(GL20.GL_BLEND);													// Enable transparent blending
			Gdx.gl.glBlendFunc(GL20.GL_SRC_ALPHA, GL20.GL_ONE_MINUS_SRC_ALPHA);				// Use alpha channel to blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the shape Renderer
			shapeRenderer.setColor(new Color(0f, 0f, 0f, 0.7f));							// Set the color
	        shapeRenderer.rect(765, 230, 375, 290);											// Render a rectangle
	        shapeRenderer.setColor(120 / 255.0f, 100 / 255.0f, 30 / 255.0f, 0.8f);			// Set a different color
	        shapeRenderer.rect(765, 230+(2-this.menuSelection)*100, 375, 90);				// Render the selected element
	        shapeRenderer.end();															// End the shape renderer
	        Gdx.gl.glDisable(GL20.GL_BLEND);												// Deactivate the transparency blend
			spriteBatch.begin();															// Start the sprite batch
			spriteBatch.draw(title,420,654);												// Render the title screen
			gameFont.draw(spriteBatch, "New Game", 820, 500);								// Render the first menu option
			gameFont.draw(spriteBatch, "High Score", 785, 400);								// Render the second menu option
			gameFont.draw(spriteBatch, "Quit Game", 802, 300);								// Render the third menu option
			spriteBatch.end();																// End the sprite batch
			break;
		case SCORE_TABLE:																	// Draw for the SCORE TABLE
			Gdx.gl.glEnable(GL20.GL_BLEND);													// Enable transparent blending
			Gdx.gl.glBlendFunc(GL20.GL_SRC_ALPHA, GL20.GL_ONE_MINUS_SRC_ALPHA);				// Use alpha channel to blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the shape renderer
			shapeRenderer.setColor(new Color(0f, 0f, 0f, 0.7f));							// Set the color
	        shapeRenderer.rect(0, 0, 1960, 1080);											// Render a rectangle
	        shapeRenderer.end();															// End the shape renderer
	        Gdx.gl.glDisable(GL20.GL_BLEND);												// Deactivate the transparency blend
			spriteBatch.begin();															// Start the sprite batch
			gameFont.draw(spriteBatch, "HIGH SCORES", 750, 980);							// Render the title
			for(int scoreCounter = 0; scoreCounter < MAX_DISPLAY_SCORE && scoreCounter < this.highScores.size();scoreCounter++) {				// Loop on the high scores
				gameFont.draw(spriteBatch, this.highScores.get(scoreCounter).getName(), 720, 880 - (scoreCounter * 100));						// Draw the name
				gameFont.draw(spriteBatch, Integer.toString(this.highScores.get(scoreCounter).getScore()), 920, 880 - (scoreCounter * 100));	// Draw the score
			}
			gameFont.draw(spriteBatch, "Press ENTER to return to the Title Screen.", 250, 100);													// Draw the instructions
			spriteBatch.end();																// End the sprite batch
			break;
		case NEXT_LEVEL:																	// Draw the NEXT_LEVEL
			Gdx.gl.glEnable(GL20.GL_BLEND);													// Enable transparent blending
			Gdx.gl.glBlendFunc(GL20.GL_SRC_ALPHA, GL20.GL_ONE_MINUS_SRC_ALPHA);				// Use alpha channel to blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the shape renderer
			shapeRenderer.setColor(new Color(0f, 0f, 0f, 0.7f));							// Set the color
	        shapeRenderer.rect(530, 400, 850, 310);											// Render a rectangle
	        shapeRenderer.end();															// End the shape renderer
	        Gdx.gl.glDisable(GL20.GL_BLEND);												// Deactivate the transparency blend
			spriteBatch.begin();															// Start the sprite batch
			gameFont.draw(spriteBatch, "LEVEL " + this.level + " COMPLETED!", 650, 680);	// Render the completed message
			gameFont.draw(spriteBatch, "Press ENTER to continue.", 550, 480);				// Render the instructions
			spriteBatch.end();																// End the sprite batch
			break;
		case GAME_OVER:																		// Draw the GAME_OVER
			Gdx.gl.glEnable(GL20.GL_BLEND);													// Enable transparent blending
			Gdx.gl.glBlendFunc(GL20.GL_SRC_ALPHA, GL20.GL_ONE_MINUS_SRC_ALPHA);				// Use alpha channel to blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the Shape renderer
			shapeRenderer.setColor(new Color(0f, 0f, 0f, 0.7f));							// Set the color
	        shapeRenderer.rect(410, 200, 1110, 510);										// Render a rectangle
	        shapeRenderer.end();															// End the shape renderer
	        Gdx.gl.glDisable(GL20.GL_BLEND);												// Deactivate the transparency blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the Shape renderer
	        shapeRenderer.setColor(120 / 255.0f, 100 / 255.0f, 30 / 255.0f, 0.5f);			// Set the color
	        shapeRenderer.rect(1161+this.initialSelected*34, 428, 30, 60);					// Draw the selector for the initials
	        shapeRenderer.end();															// End the shape renderer
			spriteBatch.begin();															// Begin the sprite batch
			gameFont.draw(spriteBatch, "GAME OVER!", 800, 680);								// Render the title
			gameFont.draw(spriteBatch, "Your score was: " + this.currentLevel.getCurrentScore(), 650, 580);		// Render the score
			gameFont.draw(spriteBatch, "Your initials: " + String.valueOf(this.initials), 650, 480);			// Render the initials
			gameFont.draw(spriteBatch, "Press ENTER to save your score.", 450, 280);							// render the instructions
			spriteBatch.end();																// End the sprite batch
			break;
		case PAUSE:																			// Draw the PAUSE
			Gdx.gl.glEnable(GL20.GL_BLEND);													// Enable transparent blending
			Gdx.gl.glBlendFunc(GL20.GL_SRC_ALPHA, GL20.GL_ONE_MINUS_SRC_ALPHA);				// Use alpha channel to blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the Shape renderer
			shapeRenderer.setColor(new Color(0f, 0f, 0f, 0.7f));							// Set the color
	        shapeRenderer.rect(0, 0, 1960, 1080);											// Render a rectangle
	        shapeRenderer.end();															// End the shape renderer
	        Gdx.gl.glDisable(GL20.GL_BLEND);												// Deactivate the transparency blend
	        spriteBatch.begin();															// Begin the sprite batch
	        gameFont.draw(spriteBatch, "PAUSED", 820, 580);									// Render the title
	        spriteBatch.end();																// End the sprite batch
			break;
		case GAMEPLAY:																		// Draw the GAMEPLAY
			Gdx.gl.glEnable(GL20.GL_BLEND);													// Enable transparent blending
			Gdx.gl.glBlendFunc(GL20.GL_SRC_ALPHA, GL20.GL_ONE_MINUS_SRC_ALPHA);				// Use alpha channel to blend
			shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);							// Begin the Shape renderer
			shapeRenderer.setColor(new Color(0f, 0f, 0f, 0.7f));							// Set the color
	        shapeRenderer.rect(10, 880, 700, 160);											// Render a rectangle
	        shapeRenderer.end();															// End the sprite batch
	        Gdx.gl.glDisable(GL20.GL_BLEND);												// Deactivate the transparency blend
			spriteBatch.begin();															// Begin the sprite batch
		    this.currentLevel.drawPlayer(spriteBatch, true);								// Render the player only if it is falling behind
		    this.currentLevel.drawEnemies(spriteBatch, true);								// Render the enemies falling behind
		    spriteBatch.end();																// End the sprite batch
			polyBatch.begin();																// Begin the poly batch
			this.currentLevel.drawBlocks(polyBatch);										// Draw the platforms
		    polyBatch.end();																// End the poly batch
		    spriteBatch.begin();															// Begin the sprite batch
		    this.currentLevel.drawPlayer(spriteBatch, false);								// Render the player if it is in fron of the platforms
		    this.currentLevel.drawEnemies(spriteBatch, false);								// Render the enemies that are in fron of the platforms
		    gameFont.draw(spriteBatch, "Score: "+(this.currentLevel.getCurrentScore()), 40,1020);		// Render the current score
		    gameFont.draw(spriteBatch, "Level: "+this.level, 40,980);									// Render the current level
		    gameFont.draw(spriteBatch, "Lives: "+this.currentLevel.playersLives(), 40,940);				// Render the current lives
		    spriteBatch.end();																// End the sprite batch
			break;
		default:																			// No more states to handle
			break;
		}  
	}
	/**
	 * Loads a level depending on the current level number.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	private void loadLevel() {
		if(this.currentLevel != null) {												// Make sure the previous level has been instantiated
			this.currentLevel.dispose();											// Free the graphical memory
		}	
		currentLevel = new QbertLevel(this.level, this.player, this.score);			// Create a new level
	}
	/**
	 * Loads the scores from a binary file.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	private void loadScores() {														
		this.highScores = new LinkedList<PlayerRecord>();							// Create a new empty high score list
		int bytesRead = -1, score = -1, nameCounter, scoreCounter, bytesCounter;	// Declare all the variables that will be used that are integers
		long fileSize;																// Declare the file size
		String nameInString;														// Declare a String variable for the name
		byte[] nameInBytes, scoreIntBytes, allBytes;								// Declare all the byte arrays that will be used
		char[] name;																// Declare the char array for the name
		try {
			InputStream inputStream = new FileInputStream("scores.dat");			// Create a input stream for the file
			fileSize = new File("scores.dat").length();								// get the file's length
			allBytes = new byte[(int) fileSize];									// Create a byte array to store all the bytes
			bytesRead = inputStream.read(allBytes);									// Read the file and store it into allBytes, set the number of bytes read
			inputStream.close();													// Close the file
			nameInBytes = new byte[PlayerRecord.NAME_SIZE];							// Create a byte array to store the name
			scoreIntBytes = new byte[4];											// Create a byte array to store the score
			name = new char[PlayerRecord.NAME_SIZE];								// Create the char array for the name
			for(bytesCounter = 0;bytesCounter < bytesRead;bytesCounter += 7) {		// Loop for each record
				for(nameCounter = 0; nameCounter < PlayerRecord.NAME_SIZE;nameCounter++) {		// Loop three times, one for each char of the name
					nameInBytes[nameCounter] = allBytes[bytesCounter+nameCounter];				// Store the char into the name byte array
				}
				nameInString = new String(nameInBytes);								// Convert the byte array to a string
				name = nameInString.toCharArray();									// Convert the string to a char array
				for(scoreCounter = 0; scoreCounter < 4;scoreCounter++) {			// Loop four times for each byte that is part of the score
					scoreIntBytes[scoreCounter] = allBytes[bytesCounter+PlayerRecord.NAME_SIZE+scoreCounter];		// Store the byte into the score byte array
				}
				IntBuffer intBuffer = ByteBuffer.wrap(scoreIntBytes).asIntBuffer();	// Put the byte array into an int buffer
				score = intBuffer.get();											// Convert the information to an int
				this.highScores.add(new PlayerRecord(name, score));					// Add the read score
			}
		} catch (ArrayIndexOutOfBoundsException exception) {						// Handle the exception in case there's an index out of bounds in an array
        	System.out.println("The file doesn't have the correct format");			// Print in console the message
        	exception.printStackTrace();											// Print the stack trace to fin which line had the problem
        } catch(FileNotFoundException exception) {									// Handle a file not found exception
			System.out.println("There's no saved file, it will be created when saving a new score.");		// Print the message
		} catch (IOException ex) {													// Handle any other IO exception
            ex.printStackTrace();													// Print the stack trace to see what happened
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
	private void saveScore(char[] name, int score) {
		this.addScore(new PlayerRecord(name, score));							// Add the score to the list
		try {
			File fileToDelete = new File("scores.dat");							// Get the current file to delete it
			fileToDelete.delete();												// Delete the current file
			File newFile = new File("scores.dat");								// Create the new file
			OutputStream outputStream = new FileOutputStream(newFile);			// Create a new output scream with the file
			for(int scoreCounter = 0; scoreCounter < MAX_DISPLAY_SCORE && scoreCounter < this.highScores.size();scoreCounter++) {	// Loop through the score
				outputStream.write(this.highScores.get(scoreCounter).getName().getBytes(), 0, 3);									// Write the initials
				outputStream.write(ByteBuffer.allocate(4).putInt(this.highScores.get(scoreCounter).getScore()).array(), 0, 4);		// Write the score
			}
			outputStream.close();												// Close the file
		} catch (ArrayIndexOutOfBoundsException exception) {					// Handle the exception in case there's an index out of bounds in an array
        	System.out.println("Error writing into the file.");					// Print in console the message
        	exception.printStackTrace();										// Print the stack trace to fin which line had the problem
        } catch(IOException ex) {												// Handle any other IO exception
			ex.printStackTrace();												// Print the stack trace to see what happened
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
	private void addScore(PlayerRecord score) {
		boolean foundRank = false;																			// Declare a false boolean to see if a location for the new score has been found
		int rank = 0;																						// Declare an int for the new score's position
		for(int scoreCounter = 0; scoreCounter < this.highScores.size() && !foundRank;scoreCounter++) {		// Loop through the score list until it is over or an adequate rank has been found
			if(this.highScores.get(scoreCounter).getScore() < score.getScore()) {							// Check if the new score is higher than the current score
				foundRank = true;																			// We have found an appropriate rank
			} else {
				rank++;																						// Otherwise increment the position
			}
		}
		this.highScores.add(rank, score);																	// Add the score in the found position
	}
}
