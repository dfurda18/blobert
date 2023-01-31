'''
This class represents A Qbert player.

@author: Dario Urdapilleta
@version 1.0
@since: 13 nov. 2022
'''
import pygame.math as Math
from QbertPackage import QbertBeing

class QbertPlayer(QbertBeing.QbertBeingClass):
    '''
    Variables:
    lives: The player's lives
    '''


    def __init__(self, texture):
        '''
        This class represents A Qbert player.
        @param self The current object
        @param texture The spritesheet
        @return A new instance of the QbertPlayer

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        super().__init__(texture, 0, 8)     # Call the parent's method
        self.lives = 3                      # Set the initial lives to 3
        
    def getLives(self):
        '''
        Return the amount of lives the player has.
        @param self The current object
        @return The amount of lives the player has.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.lives      # Return the amount of lives the player has.
    
    def loseLives(self):
        '''
        Make the player lose a life
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.lives = self.lives - 1           # Reduce the player's lives by 1
        
    def isDead(self):
        '''
        Return true if the player is dead
        @param self The current object
        @return True if the player is dead.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.lives <= 0     # Return true if the player's lives are less or equal than 0
    
    def respawn(self):
        '''
        Respawns the player in the top position.
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        super().respawn()               # Call the parent respawn method
        self.setPosition(Math.Vector2(0,0))  # Set the player's position to the top position
        
    def oneUp(self):
        '''
        Adds one life to the player
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.lives = self.lives + 1           # Increase the player's lives by 1