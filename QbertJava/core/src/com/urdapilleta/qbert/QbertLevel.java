package com.urdapilleta.qbert;

import java.util.Random;

import com.badlogic.gdx.graphics.g2d.PolygonSpriteBatch;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
/**
 * This class represents A Qbert level that manages all the gameplay.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class QbertLevel {
	private final float initialX = 850;		// The graphical initial x position
	private final float initialY = 650;		// The graphical initial y position
	private final float initialXG = 900;	// The graphical initial x position
	private final float initialYG = 750;	// The graphical initial y position
	private QbertBoard clearBoard;			// The game board
	private QbertBoard enemyBoard;			// The enemy board
	private QbertPlayer player;				// The player
	private QbertEnemy[] enemies;			// The enemy list
	private BoardBlock[] blocks;			// The graphical version of the blocks
	private Random random;					// A random generator
	private short level;					// The level number
	private int currentScore;				// The current score
	/**
	 * Creates a new QbertLevel given a player and the previous score.
	 * @param level The level number.
	 * @param player The player.
	 * @param score The previous score.
	 * @return A new instance of the QbertLevel
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public QbertLevel(short level, QbertPlayer player, int score) {
		this.random = new Random();									// Instantiate the random generator
		int numberOfEnemies = (int)Math.floor(level/3) + 1;			// Calculate the number of enemies.
		char lineCounter;											// Declare the counter for lines
		char position = 0;											// Declare the initial position and set it to 0
		this.level = level;											// Set the level number
		this.currentScore = score;									// Set the current score
		this.clearBoard = new QbertBoard();							// Create the game board
		this.enemyBoard = new QbertBoard();							// Create the enemy board
		this.player = player;										// Set the player
		this.blocks = new BoardBlock[clearBoard.getSize()];			// Create the block array
		for(char boardCounter = 0; boardCounter < QbertBoard.size; boardCounter++) {					// Loop through the board
			for(lineCounter = 0;lineCounter < this.clearBoard.size(boardCounter);lineCounter++) {		// Loop through each line
				blocks[position] = new BoardBlock(this.getBlockLocation(boardCounter, lineCounter));	// Create a new block in the next position
				position++;											// Increase the position counter
			}
		}
		this.enemies = new QbertEnemy[numberOfEnemies];				// Create the enemy array
		for(int enemyCounter = 0;enemyCounter < this.enemies.length;enemyCounter++) {					// Loop through the enemy array
			this.enemies[enemyCounter] = new QbertEnemy();												// Create a new enemy
		}
		this.player.respawn();										// Respawn the player
	}
	/**
	 * Disposes of all the graphical elements.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void dispose() {
		for(int blockCounter = 0; blockCounter < this.blocks.length;blockCounter++) {		// Loop through the blocks
			this.blocks[blockCounter].dispose();											// Delete each block
		}
		for(int enemyCounter = 0; enemyCounter < this.enemies.length; enemyCounter++) {		// Loop through the enemies
			this.enemies[enemyCounter].dispose();											// Delete the enemy
		}
	}
	/**
	 * Returns the current score.
	 * @return The current score.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int getCurrentScore() {
		return this.currentScore;		// Return the current score
	}
	/**
	 * Returns if the game level has been completed.
	 * @return True if the player has cleared a board.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean gameCompleted() {
		return this.clearBoard.isCompleted();	// Return true if the game board is completed
	}
	/**
	 * Moves the player to a direction.
	 * @param direction The direction to move the player.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void movePlayer(int direction) {
		if(!this.player.isMoving()) {			// Make sure the player is not moving
			this.player.move(direction);		// Move the player to the direction
		}
	}
	/**
	 * Moves the enemy to a random direction.
	 * @param enemy The enemy to move.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void moveEnemy(QbertEnemy enemy) {
		int direction = this.random.nextInt(4);		// Calculate a random direction
		if(!enemy.isMoving()) {						// Make sure the enemy is not moving
			enemy.move(direction);					// Move the enemy to the random direction
		}
	}
	/**
	 * Converts a board position into a graphical position.
	 * @param boardCounter The x position.
	 * @param lineCounter The y position
	 * @return A Vector2 with the graphical position.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	private Vector2 getBlockLocation(int boardCounter, int lineCounter) {
		Vector2 location = new Vector2(this.initialX+(85*lineCounter)-(85*boardCounter),this.initialY-(110*lineCounter)-(110*boardCounter));		// Create the vector witht he graphical values
		return location;		// Return the vector
	}
	/**
	 * Draws all the blocks
	 * @param polyBatch The Polygon Batch that draws the Blocks
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void drawBlocks(PolygonSpriteBatch polyBatch) {
		char position = 0;							// Declare the iteration for the board array
		char lineCounter;							// Declare the iterator for the lines
		for(char boardCounter = 0; boardCounter < QbertBoard.size; boardCounter++) {										// Loop through the lines
			for(lineCounter = 0;lineCounter < this.clearBoard.size(boardCounter);lineCounter++) {							// Loop through the cells of each line
				this.blocks[position].draw(polyBatch, this.clearBoard.getValue(new Vector2(boardCounter, lineCounter)));	// Draw the block with the value of the corresponding cell
				position++;																									// Increase the position
			}
		}
	}
	/**
	 * Returns the player's lives
	 * @return The player's lives.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int playersLives() {
		return this.player.getLives();	// Return the player's lives
	}
	/**
	 * Updates the level.
	 * @param timeSlice The time that passed since last update.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void update(float timeslice) {
		this.updateBoards();				// Update the boards
		this.updateEnemies(timeslice);		// Update the enemies
		this.updatePlayer();				// Update the player
		this.checkCollisions();			// Calculate the collisions
	}
	/**
	 * Updates the player
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void updatePlayer() {
		if(this.player.needsRespawn()) {											// Check if the player needs to respawn
			this.player.respawn();													// Respawn the player
		}
		if(this.player.landed() && this.player.isMoving()) {						// Check if the player landed and is moving
			if (this.clearBoard.isInsideBoard(this.player.getPosition())) {			// Check if the player is inside the board
				if(this.level % 2 == 1) {											// Switch between logics, the block will turn off in even levels
					if(!this.clearBoard.getValue(this.player.getPosition())) {		// Check if the block is off
						this.clearBoard.setValue(this.player.getPosition(), true);	// Turn it on
						this.currentScore += 20 * (Math.floor(this.level/10) + 1);	// Add score
					}
				} else {															// When the board can turn off
					this.clearBoard.setValue(this.player.getPosition(), !this.clearBoard.getValue(this.player.getPosition()));		// Switch the block state
					this.currentScore += 20 * (Math.floor(this.level/10) + 1);		// Add the score
				}
				this.player.land();													// Land the player
			} else {
				this.player.dropOff();												// Make the player fall if it is outside the board
				this.player.loseLives();											// Make the player lose a life
			}
		}
	}
	/**
	 * Updates the boards.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void updateBoards() {
		this.enemyBoard.clear();															// Clear the enemy board
		for(int enemyCounter = 0; enemyCounter < this.enemies.length; enemyCounter++) {		// Loop through the enemies
			if(this.enemies[enemyCounter].isStanding()) {									// Check if the enemy is standing
				this.enemyBoard.setValue(this.enemies[enemyCounter].getPosition(), true);	// Set the board true in the enemy's position
			}
		}
	}
	/**
	 * Updates the enemies.
	 * @param timeSlice The time that passed since last update.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void updateEnemies(float timeSlice) {
		for(int enemyCounter = 0; enemyCounter < this.enemies.length; enemyCounter++) {				// Loop through the enemies
			this.enemies[enemyCounter].update(timeSlice);											// Update the enemy
			if(this.enemies[enemyCounter].needsRespawn()) {											// Check if the enemy needs to respawn
				this.enemies[enemyCounter].respawn();												// Respawn the enemy
				this.enemies[enemyCounter].setPosition(this.getRandomMapPosition());				// Set the enemy position to a random position in the map
			}
			if(this.enemies[enemyCounter].isReadyToMove()) {										// Check if the enemy is ready to move
				this.moveEnemy(this.enemies[enemyCounter]);											// Move the enemy
			}
			if(this.enemies[enemyCounter].landed() && this.enemies[enemyCounter].isMoving()) {		// Check if the enemy landed and is moving
				if (this.enemyBoard.isInsideBoard(this.enemies[enemyCounter].getPosition())) {		// Check if the position is inside the board
					this.enemies[enemyCounter].land();												// Land the enemy
				} else {
					this.enemies[enemyCounter].dropOff();											// Make the enemu fall
				}
			}
		}
	}
	/**
	 * Checks the collisions.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void checkCollisions() {
		if(!this.player.isMoving() && this.enemyBoard.getValue(this.player.getPosition())) {	// Check the player is not moving and the board is occupied at the players position
			this.player.hit();				// Hit the player
			this.player.loseLives();		// Make the player lose lives
			for(int enemyCounter = 0; enemyCounter < this.enemies.length; enemyCounter++) {									// Loop through the enemies
				if(this.enemies[enemyCounter].getPosition().x == 0 && this.enemies[enemyCounter].getPosition().y == 0) {	// Check if the enemy is at the player's position
					this.enemies[enemyCounter].hit();																		// Hit the enemy
				}
			}
		}
	}
	/**
	 * Draws the player.
	 * @param spriteBatch The sprite batch that renders the player's sprites.
	 * @param before A variable that tells if the method was called before the block rendering.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void drawPlayer(SpriteBatch spriteBatch, boolean before) {
		if(before && this.player.isFalling())										// If drawn before and the player is falling
		{
			this.player.draw(spriteBatch, this.getGraphicPosition(this.player));	// Draw the player
		} else if (!before && !this.player.isFalling()) {							// If after and the player is not
			this.player.draw(spriteBatch, this.getGraphicPosition(this.player));	// Draw the player
		}
	}
	/**
	 * Draws the enemies.
	 * @param spriteBatch The sprite batch that renders the enemies' sprites.
	 * @param before A variable that tells if the method was called before the block rendering.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void drawEnemies(SpriteBatch spriteBatch, boolean before) {
		for(int enemyCounter = 0; enemyCounter < this.enemies.length; enemyCounter++) {									// Loop though the enemies
			if(this.enemies[enemyCounter].canBeDrawn()) {																// Check if the enemy can be drawn
				if(before && this.enemies[enemyCounter].isFalling()) {													// Check if drawn before and the enemy is falling
					this.enemies[enemyCounter].draw(spriteBatch, this.getGraphicPosition(this.enemies[enemyCounter]));	// Draw the enemy
				} else if (!before && !this.enemies[enemyCounter].isFalling()) {										// Check if drawn after and is not falling
					this.enemies[enemyCounter].draw(spriteBatch, this.getGraphicPosition(this.enemies[enemyCounter]));	// Draw the enemy
				}
			}
		}
	}
	/**
	 * Converts a board position into graphical position.
	 * @param being The being to draw.
	 * @return The position in graphical coordinates.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public Vector2 getGraphicPosition(QbertBeing being) {
		Vector2 location = new Vector2(initialXG+(85*being.getPosition().y)-(85*being.getPosition().x)+(85*being.getJumpMovement().y)-(85*being.getJumpMovement().x),	// Calculate the new vector with the graphical coordinates
				initialYG-(110*being.getPosition().y)-(110*being.getPosition().x)-(110*being.getJumpMovement().y)-(110*being.getJumpMovement().x));
		return location;		// Return the calculated Vector2
	}
	/**
	 * Gets a random map position.
	 * @return A Vector2 with a random valid position in the map.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	private Vector2 getRandomMapPosition() {
		int x = this.random.nextInt(this.enemyBoard.getWidth());		// Get a random x position
		int y = this.random.nextInt(this.clearBoard.getWidth()-x);		// Get a random y position
		return new Vector2(x,y);										// Create the vector and return it.
	}
}