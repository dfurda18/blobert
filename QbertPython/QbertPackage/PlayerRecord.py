'''
Created on 12 nov. 2022

This class represents a player record.

Created on 21 sep. 2022

@author: Dario Urdapilleta
@version 1.0
@since 12 nov. 2022
'''
class PlayerRecord(object):
    '''
    Variables:
    name: The name
    score: The score
    '''

    def __init__(self, name, score):
        '''
        The PlayerRecord constructor.
        
        @param self The current object
        @param name The record's name.
        @param score The record's score
        @return A new instance of the PlayerRecord class.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.name = [None] * 3                              # Declare the name
        for nameCounter in range(3):                        # Loop through the name
            self.name[nameCounter] = name[nameCounter]      # Sets the times guessed to 0
        self.score = score                                  # Set the current guesses as an array of length of the word.
    def getName(self):
        '''
        Returns the record name as a String.

        @param self The current object
        @return The record's name as a String.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        string_name = ""                        # Make an empty name
        for initial in self.name:               # Loop through the initials
            string_name += initial              # Add the initial to the name
        return string_name                      # Set the current guesses as an array of length of the word.
    def getScore(self):
        '''
        Returns the record score.

        @param self The current object
        @return The record's score.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        return int(self.score)                       # Set the current guesses as an array of length of the word.
        
        