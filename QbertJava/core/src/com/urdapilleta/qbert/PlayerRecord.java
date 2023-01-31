package com.urdapilleta.qbert;
/**
 * This class represents a player record.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class PlayerRecord {
	public static final short NAME_SIZE = 3;			// The record's name size
	private char[] name;								// The name
	private int score;									// The score
	/**
	 * PlayerRecord constructor given its name and score.
	 * @param name The record's name.
	 * @param score The record's score
	 * @return A new instance of the PlayerRecord class.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public PlayerRecord(char[] name, int score) {
		this.name = new char[NAME_SIZE];									// Create the name array
		for(int nameCounter = 0; nameCounter < NAME_SIZE; nameCounter++) {	// Loop through the array
			this.name[nameCounter] = name[nameCounter];						// Copy it from the parameter name
		}
		this.score = score;													// Set the score
	}
	/**
	 * Returns the record name as a String.
	 * @return The record's name as a String.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public String getName() {
		return String.valueOf(this.name);		// Return the record name as a string
	}
	/**
	 * Returns the record score.
	 * @return The record's score.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int getScore() {
		return this.score;						// Return the score
	}
}
