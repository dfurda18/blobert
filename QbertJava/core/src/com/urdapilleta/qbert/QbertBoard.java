package com.urdapilleta.qbert;

import com.badlogic.gdx.math.Vector2;
/**
 * This class represents A Qbert board.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class QbertBoard {
	public static final char size = 6;			// Constant board size of 6
	private boolean[][] board;					// The logical board
	/**
	 * Creates a new instance of a QbertBoard.
	 * @return A new instance of the QbertBoard
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public QbertBoard ()
	{
		board = new boolean[size][];			// Instantiate the board
		this.clear();							// Set all values to false
	}
	/**
	 * Returns the board size.
	 * @return The boards total size.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int getSize() {
		int size = 0;									// Declare the variable to store the return value
		for(int boardCounter = 0; boardCounter < this.board.length; boardCounter++) {		// loop through the board
			size += board[boardCounter].length;			// Add the line's size to the count
		}
		return size;									// Return the count
	}
	/**
	 * Returns the board's number of lines.
	 * @return The board's number of lines.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int getWidth() {
		return this.board.length;		// Return the board's number of lines
	}
	/**
	 * Returns a specific line's size.
	 * @param row The line that is inquired.
	 * @return The number of elements the specific line has.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public int size(int row) {
		return this.board[row].length; 	// Return the length of the specific line
	}
	/**
	 * Returns the board's value given a location.
	 * @param position A Vector2 with the location
	 * @return The value of that position in this board.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean getValue(Vector2 position) {
		boolean active = false;						// Declare the variable where the result will be stored
		if(this.isInsideBoard(position)) {			// Make sure the position is inside the board
			active = this.board[(int)position.x][(int)position.y];	// Check the value of the board in the specified position
		}
		return active;								// Return the value
	}
	/**
	 * Sets a specified value in the specified position.
	 * @param position A Vector2 with the position.
	 * @param value The value to set.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void setValue(Vector2 position, boolean value) {
		if(this.isInsideBoard(position)) {							// Make sure the position is inside the board
			this.board[(int)position.x][(int)position.y] = value;	// Set the value in the specified position
		}
	}
	/**
	 * Notifies if a position is inside the board.
	 * @param position A Vector2 with the position.
	 * @param value True if the position is inside the board.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean isInsideBoard(Vector2 position) {
		return position.x >= 0 											// Return true if the position is not negative or over the board
				&& position.x < this.board.length 
				&& position.y >= 0 
				&& position.y < this.board[(int)position.x].length;
	}
	/**
	 * Return true if the board has all values as true.
	 * @return True if all the elements in the board are true.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public boolean isCompleted() {
		boolean isCompleted = true;									// Declare the return variable
		char lineCounter;											// Declare the itration variable
		for(char boardCounter = 0; boardCounter < this.board.length && isCompleted;boardCounter++) {			// Loop through the lines or stop if one cell is false
			for(lineCounter = 0;lineCounter < this.board[boardCounter].length && isCompleted;lineCounter++) {	// Loop through the line elements or stop if one cell is false
				if(!this.board[boardCounter][lineCounter]) {		// Check if the cell false
					isCompleted = false;							// Found a cell that is false
				}
			}
		}
		return isCompleted;											// Return the finding
	}
	/**
	 * Builds the board and sets all the board's cells to false.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void clear() {
		char lineCounter;																	// Declare the iteration variable
		for(char boardCounter = 0; boardCounter < size; boardCounter++) {					// Loop through the lines
			board[boardCounter] = new boolean[size - boardCounter];							// Create the line
			for(lineCounter = 0;lineCounter < board[boardCounter].length;lineCounter++) {	// Loop through the line's cells
				board[boardCounter][lineCounter] = false;									// Set the cell to false
			}
		}
	}
}