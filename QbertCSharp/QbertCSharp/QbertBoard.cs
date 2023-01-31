using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace QbertCSharp
{
    /**
     * This class represents A Qbert board.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class QbertBoard
    {
        private const int SIZE = 6;
        private BitArray[] board;               // The logical board
        /**
         * Creates a new instance of a QbertBoard.
         * @return A new instance of the QbertBoard
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public QbertBoard()
        {
            board = new BitArray[QbertBoard.SIZE];            // Instantiate the board
            this.Clear();                       // Set all values to false
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
        public int GetSize()
        {
            int size = 0;                                   // Declare the variable to store the return value
            for (int boardCounter = 0; boardCounter < this.board.Length; boardCounter++)     // loop through the board
            {
                size += board[boardCounter].Length;         // Add the line's size to the count
            }
            return size;                                    // Return the count
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
        public int GetWidth()
        {
            return this.board.Length;       // Return the board's number of lines
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
        public int Size(int row)
        {
            return this.board[row].Length;  // Return the length of the specific line
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
        public bool GetValue(Vector2 position)
        {
            bool active = false;                                        // Declare the variable where the result will be stored
            if (this.IsInsideBoard(position))                           // Make sure the position is inside the board
            {
                active = this.board[(int)position.X][(int)position.Y];  // Check the value of the board in the specified position
            }
            return active;                                              // Return the value
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
        public void SetValue(Vector2 position, bool value)
        {
            if (this.IsInsideBoard(position))
            {                           // Make sure the position is inside the board
                this.board[(int)position.X][(int)position.Y] = value;   // Set the value in the specified position
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
        public bool IsInsideBoard(Vector2 position)
        {
            return position.X >= 0                                          // Return true if the position is not negative or over the board
                    && position.X < this.board.Length
                    && position.Y >= 0
                    && position.Y < this.board[(int)position.X].Length;
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
        public bool IsCompleted()
        {
            bool isCompleted = true;                                    // Declare the return variable
            short lineCounter;                                           // Declare the itration variable
            for (short boardCounter = 0; boardCounter < this.board.Length && isCompleted; boardCounter++)
            {           // Loop through the lines or stop if one cell is false
                for (lineCounter = 0; lineCounter < this.board[boardCounter].Length && isCompleted; lineCounter++)
                {   // Loop through the line elements or stop if one cell is false
                    if (!this.board[boardCounter][lineCounter])
                    {       // Check if the cell false
                        isCompleted = false;                            // Found a cell that is false
                    }
                }
            }
            return isCompleted;                                         // Return the finding
        }
        /**
         * Builds the board and sets all the board's cells to false.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Clear()
        {
            short lineCounter;                                                                  // Declare the iteration variable
            for (short boardCounter = 0; boardCounter < 6; boardCounter++)                      // Loop through the lines
            {                  
                board[boardCounter] = new BitArray(6 - boardCounter);                           // Create the line
                for (lineCounter = 0; lineCounter < board[boardCounter].Length; lineCounter++)  // Loop through the line's cells
                {   
                    board[boardCounter][lineCounter] = false;                                   // Set the cell to false
                }
            }
        }
    }
}
