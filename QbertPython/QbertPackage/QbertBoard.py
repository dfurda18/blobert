'''
This class represents A Qbert board.

@author: Dario Urdapilleta
@version 1.0
@since: 13 nov. 2022
'''
class QbertBoard(object):
    '''
    Variables:
    board: The logical board
    '''

    def __init__(self):
        '''
        Creates a new instance of a QbertBoard.
        @param self The current object
        @return A new instance of the QbertBoard
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.board = [None] * 6     # Instantiate the board
        self.clear()                # Set all values to false
        
    def getSize(self):
        '''
        Returns the board size.
        @param self The current object
        @return The boards total size.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        size = 0                                            # Declare the variable to store the return value
        for boardCounter in range(len(self.board)):         # loop through the board
            size = size + len(self.board[boardCounter])     # Add the line's size to the count
        return size                                         # Return the count
    
    def getWidth(self):
        '''
        Returns the board's number of lines.
        @param self The current object
        @return The board's number of lines.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return len(self.board)      # Return the board's number of lines
    
    def size(self, row):
        '''
        Returns a specific line's size.
        @param self The current object
        @param row The line that is inquired.
        @return The number of elements the specific line has.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return len(self.board[row])     # Return the length of the specific line
    
    def getValue(self, position):
        '''
        Returns the board's value given a location.
        @param self The current object
        @param position A Vector2 with the location
        @return The value of that position in this board.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        active = False                                  # Declare the variable where the result will be stored
        if self.isInsideBoard(position):                # Make sure the position is inside the board
            active = self.board[int(position.x)][int(position.y)] # Check the value of the board in the specified position
        return active                                   # Return the value
    
    def setValue(self, position, value):
        '''
        Sets a specified value in the specified position.
        @param self The current object
        @param position A Vector2 with the position.
        @param value The value to set.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        if self.isInsideBoard(position):                # Make sure the position is inside the board
            self.board[int(position.x)][int(position.y)] = value  # Set the value in the specified position
            
    def isInsideBoard(self, position):
        '''
        Notifies if a position is inside the board.
        @param self The current object
        @param position A Vector2 with the position.
        @param value True if the position is inside the board.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return position != None and position.x >= 0 and position.x < len(self.board) and position.y >= 0 and position.y < len(self.board[int(position.x)])        # Return true if the position is not negative or over the board
    
    def isCompleted(self):
        '''
        Return true if the board has all values as true.
        @param self The current object
        @return True if all the elements in the board are true.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        isCompleted = True                                              # Declare the return variable
        for boardCounter in range(len(self.board)):                     # Loop through the lines or stop if one cell is false
            for lineCounter in range(len(self.board[boardCounter])):    # Loop through the line elements or stop if one cell is false
                if not self.board[boardCounter][lineCounter]:           # Check if the cell false
                    isCompleted = False                                 # Found a cell that is false
        return isCompleted                                              # Return the finding
    
    def clear(self):
        '''
        Builds the board and sets all the board's cells to false.
        @param self The current object
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        for boardCounter in range(len(self.board)):                     # Loop through the lines or stop if one cell is false
            self.board[boardCounter] = [None] * (6 - boardCounter)      # Create the line
            for lineCounter in range(len(self.board[boardCounter])):    # Loop through the line elements or stop if one cell is false
                self.board[boardCounter][lineCounter] = False           # Set the cell to false
    