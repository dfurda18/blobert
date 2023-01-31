package com.urdapilleta.qbert;
/**
 * This class represents A Qbert enemy.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class QbertEnemy extends QbertBeing {
	private final long lowestTimeToAct = 220;	// The shortest time to make a movement
	private final long highestTimeToAct = 450;	// The longest time to make a movement
	private long timeToAct;						// The time to wait for the next act
	private long timer;							// The timer
	private boolean readyToMove;				// A variable if it is ready to move
	/**
	 * Creates a new instance of a QbertEnemy.
	 * @return A new instance of the QbertEnemy
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public QbertEnemy() {
		super("player.png", 5, 8, 10);			// Call the parent's class constructor
		this.timer = 0;							// Set the timer to 0
		this.readyToMove = false;				// Start by not being ready to move.
	}
	/**
	 * Notifies if the enemy is ready to move.
	 * @return True if the enemy is ready to move.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean isReadyToMove() {
		return this.readyToMove;		// Return true if the enemy is ready to move
	}
	/**
	 * Respawns the enemy.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void respawn() {
		super.respawn();		// Call the parent's respawn method
	}
	/**
	 * Updates the enemy.
	 * @param timeSlice The time passed since last update
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void update(float timeSlice) {
		if(this.getPosition() == null) {		// If the enemy doesnt have a position
			this.pleaseRespawn = true;			// Request to be spawned			
			this.resetTimeToAct();				// Reset the time to act
		}
		this.timer += timeSlice;				// Add the time slice to the timer
		if(this.timer >= this.timeToAct) {		// Check if the timer is larger than the time to act
			this.resetTimeToAct();				// Reset the time to act
			this.readyToMove = true;			// The enemy is ready to move
		}
	}
	/**
	 * Notifies if the enemy can be drawn.
	 * @return True if the enemy has a position so it can be drawn.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean canBeDrawn() {
		return this.getPosition() != null;		// Return true if the enemy has a position
	}
	/**
	 * Moves an enemy towards a direction
	 * @rparam direction The direction to move.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void move(int direction) {
		super.move(direction);		// Call the parent's move method
		this.readyToMove = false;	// The enemy is no longer ready to move
		this.resetTimeToAct();		// Reset the time to act
	}
	/**
	 * Resets the time to act
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void resetTimeToAct() {
		this.timer = 0;									// Set the timer to 0
		this.timeToAct = (long)Math.floor(Math.random() * (this.highestTimeToAct - this.lowestTimeToAct));	// Gets a random number between the difference of the shortest and longest
		this.timeToAct += this.lowestTimeToAct;			// Adds the shortest to the new time to act
	}
}