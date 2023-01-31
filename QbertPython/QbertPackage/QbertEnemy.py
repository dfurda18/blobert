'''
This class represents A Qbert enemy.

@author: Dario Urdapilleta
@version 1.0
@since: 13 nov. 2022
'''
import random
from QbertPackage import QbertBeing

class QbertEnemy(QbertBeing.QbertBeingClass):
    '''
    Variables:
    lowestTimeToAct: The shortest time to make a movement
    highestTimeToAct: The longest time to make a movement
    timeToAct: The time to wait for the next act
    timer: The timer
    readyToMove: A variable if it is ready to move
    '''
    lowestTimeToAct = 3     # The shortest time to make a movement
    highestTimeToAct = 8    # The longest time to make a movement

    def __init__(self, texture):
        '''
        Creates a new instance of a QbertEnemy.
        @param self The current object
        @param texture The spritesheet
        @return A new instance of the QbertEnemy

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        super().__init__(texture, 5, 8)
        self.timer = 0              # Set the timer to 0
        self.readyToMove = False    # Start by not being ready to move.
        self.position = None        # Set a null enemy position
        
    def isReadyToMove(self):
        '''
        Notifies if the enemy is ready to move.
        @param self The current object
        @return True if the enemy is ready to move.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.readyToMove        # Return true if the enemy is ready to move
    
    def respawn(self):
        '''
        Respawns the enemy.
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        super().respawn()        # Call the parent's respawn method
        
    def update(self, gameTime):
        super().update(gameTime)            # Call the parent update
        if not self.canBeDrawn():           # If the enemy doesn't have a position
            self.pleaseRespawn = True       # Request to be spawned
            self.resetTimeToAct()           # Reset the time to act
        self.timer = self.timer + gameTime  # Add the time slice to the timer
        if self.timer >= self.timeToAct:    # Check if the timer is larger than the time to act
            self.resetTimeToAct()           # Reset the time to act
            self.readyToMove = True         # The enemy is ready to move
            
    def canBeDrawn(self):
        '''
        Notifies if the enemy can be drawn.
        @param self The current object
        @return True if the enemy has a position so it can be drawn.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.getPosition() != None
    
    def move(self, direction):
        '''
        Moves an enemy towards a direction
        @param self The current object
        @rparam direction The direction to move.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        super().move(direction)         # Call the parent's move method
        self.readyToMove = False        # The enemy is no longer ready to move
        self.resetTimeToAct()           # Reset the time to act
        
    def resetTimeToAct(self):
        '''
        Resets the time to act
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.timer = 0                                                                              # Set the timer to 0
        self.timeToAct = random.randint(QbertEnemy.lowestTimeToAct, QbertEnemy.highestTimeToAct)    # Get the time to act as a random number