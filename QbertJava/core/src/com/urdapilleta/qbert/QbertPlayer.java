package com.urdapilleta.qbert;

import com.badlogic.gdx.math.Vector2;
/**
 * This class represents A Qbert player.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class QbertPlayer extends QbertBeing {
	private char lives;			// The player's lives
	/**
	 * Creates a new QbertPlayer.
	 * @return A new instance of the QbertPlayer
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public QbertPlayer() {
		super("player.png", 0, 8, 10);		// Call the parent's constructor method
		this.lives = 3;						// Set the initial lives to 3;
	}
	/**
	 * Return the amount of lives the player has.
	 * @return The amount of lives the player has.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int getLives() {
		return this.lives;		// Return the amount of lives the player has.
	}
	/**
	 * Make the player lose a life
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void loseLives() {
		this.lives--;			// Reduce the player's lives by 1
	}
	/**
	 * Return true if the player is dead
	 * @return True if the player is dead.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean isDead() {
		return this.lives <= 0;		// Return true if the player's lives are less or equal than 0
	}
	/**
	 * Respawns the player in the top position.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void respawn() {
		super.respawn();						// Call the parent respawn method
		this.setPosition(new Vector2(0,0));		// Set the player's position to the top position
		
	}
	/**
	 * Adds one life to the player
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void oneUp() {
		this.lives++;		// Increase the player's lives by 1
	}
}