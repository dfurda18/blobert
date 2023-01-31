'''
This class represents A Qbert level that manages all the gameplay.

@author: Dario Urdapilleta
@version 1.0
@since: 12 nov. 2022
'''
from QbertPackage import QbertPlayer
from QbertPackage import QbertBoard
from QbertPackage import QbertEnemy
from QbertPackage import BoardBlock
import random
import pygame.math as Math
import math
from pip._vendor.pyparsing.util import line
class QbertLevel(object):
    '''
    Variables:
    initialX: The graphical initial x position
    initialY: The graphical initial y position
    initialXG: The graphical initial x position
    initialYG: The graphical initial y position
    clearBoard: The game board
    enemyBoard: The enemy board
    player: The player
    enemies: The enemy list
    blocks: The graphical version of the blocks
    level: The level number
    currentScore: The current score
    '''
    initialX = 850          # The graphical initial x position
    initialY = 340          # The graphical initial y position
    initialXG = 900         # The graphical initial x position
    initialYG = 280         # The graphical initial y position

    def __init__(self, level, player, score, enemyTexture):
        '''
        Creates a new QbertLevel given a player and the previous score.
        @param self The current object
        @param level The level number.
        @param player The player.
        @param score The previous score.
        @param screen The screen
        @param enemyTexture The enemies' texture
        @return A new instance of the QbertLevel
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.level = level                                      # Set the level number
        self.currentScore = score                               # Set the current score
        self.clearBoard = QbertBoard.QbertBoard()                          # Create the game board
        self.enemyBoard = QbertBoard.QbertBoard()                          # Create the enemy board
        self.player = player                                    # Set the player
        self.blocks = [None] * self.clearBoard.getSize()        # Create the block array
        self.enemies = [None] * ((int)(level / 3) + 1)          # Create the enemy array
        position = 0                                            # Declare the initial position and set it to 0
        for boardCounter in range(self.clearBoard.size(0)):  # Loop through the board
            for lineCounter in range(self.clearBoard.size(boardCounter)):       # Loop through each line
                self.blocks[position] = BoardBlock.BoardBlock(self.getBlockLocation(boardCounter, lineCounter))        # Create a new block in the next posit
                position = position + 1                         # Increase the position
        for enemyCounter in range(len(self.enemies)):           # Loop through the enemy array
            self.enemies[enemyCounter] = QbertEnemy.QbertEnemy(enemyTexture)           # Create a new enemy
        self.player.respawn()                                   # Respawn the player
        
    def getCurrentScore(self):
        '''
        Returns the current score.
        @param self The current object
        @return The current score.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        return self.currentScore        # Return the current score
    
    def gameCompleted(self):
        '''
        Returns if the game level has been completed.
        @param self The current object
        @return True if the player has cleared a board.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        return self.clearBoard.isCompleted()        # Return true if the game board is completed
    
    def movePlayer(self, direction):
        '''
        Moves the player to a direction.
        @param self The current object
        @param direction The direction to move the player.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        if not self.player.isMoving:      # Make sure the player is not moving
            self.player.move(direction)     # Move the player to the direction
            
    def moveEnemy(self, enemy):
        '''
        Moves the enemy to a random direction.
        @param self The current object
        @param enemy The enemy to move.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        direction = random.randint(0,4)     # Calculate a random direction
        if not enemy.isMoving:            # Make sure the enemy is not moving
            enemy.move(direction)           # Move the enemy to the random direction
            
    def getBlockLocation(self, boardCounter, lineCounter):
        '''
        Converts a board position into a graphical position.
        @param self The current object
        @param boardCounter The x position.
        @param lineCounter The y position
        @return A Vector2 with the graphical position.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        location = Math.Vector2(QbertLevel.initialX + (85 * lineCounter) - (85 * boardCounter), QbertLevel.initialY + (110 * lineCounter) + (110 * boardCounter))   # Create the vector witht he graphical values
        return location     # Return the vector
    
    def drawBlocks(self, screen):
        '''
        Draws all the blocks
        @param self The current object
        @param graphicDevice The graphic device
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        position = 0                                            # Declare the iteration for the board array
        for boardCounter in range(self.clearBoard.size(0)):  # Loop through the board
            for lineCounter in range(self.clearBoard.size(boardCounter)):       # Loop through each line
                self.blocks[position].draw(screen, self.clearBoard.getValue(Math.Vector2(boardCounter, lineCounter)))       # Draw the block with the value of the corresponding cell
                position = position + 1                         # Increase the position
        
    def playersLives(self):
        '''
        Returns the player's lives
        @param self The current object
        @return The player's lives.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        return self.player.getLives()       # Return the player's lives
    
    def update(self, gameTime):
        '''
        Updates the level.
        @param self The current object
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.updateBoards()                 # Update the boards
        self.updateEnemies(gameTime)        # Update the enemies
        self.updatePlayer(gameTime)         # Update the player
        self.checkCollisions()              # Calculate the collisions
        
    def updatePlayer(self, gameTime):
        '''
        Updates the player.
        @param self The current object
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.player.update(gameTime)                                                                    # Update the player
        if self.player.needsRespawn():                                                                  # Check if the player needs to respawn
            self.player.respawn()                                                                       # Respawn the player
        if self.player.landed and self.player.isMoving:                                                 # Check if the player landed and is moving
            if self.clearBoard.isInsideBoard(self.player.getPosition()):                                # Check if the player is inside the board
                if self.level % 2 == 1:                                                                 # Switch between logics, the block will turn off in even levels
                    if not self.clearBoard.getValue(self.player.getPosition()):                         # Check if the block is off
                        self.clearBoard.setValue(self.player.getPosition(), True)                       # Turn it on
                        self.currentScore += 20 * (int)(math.floor((self.level / 10) + 1))              # Add score
                else:
                    self.clearBoard.setValue(self.player.getPosition(), not self.clearBoard.getValue(self.player.getPosition()))     # Switch the block state
                    self.currentScore += 20 * (int)(math.floor((self.level / 10) + 1))                  # Add score
                self.player.land()                                                                      # Land the player
            else:
                self.player.dropOff()                                                                   # Make the player fall if it is outside the board
                self.player.loseLives()                                                                 # Make the player lose a life
                
                
    def updateBoards(self):
        '''
        Updates the boards.
        @param self The current object
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.enemyBoard.clear()                                                             # Clear the enemy board
        for enemyCounter in range(len(self.enemies)):                                       # Loop through the enemies
            if self.enemies[enemyCounter].isStanding():                                    # Check if the enemy is standing
                self.enemyBoard.setValue(self.enemies[enemyCounter].getPosition(), True)    # Set the board true in the enemy's position
        
    def updateEnemies(self, gameTime):
        '''
        Updates the enemies.
        @param self The current object
        @param gameTime The game time
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        for enemyCounter in range(len(self.enemies)):                                           # Loop through the enemies
            self.enemies[enemyCounter].update(gameTime)                                         # Update the enemy
            if self.enemies[enemyCounter].needsRespawn():                                       # Check if the enemy needs to respawn
                self.enemies[enemyCounter].respawn()                                            # Respawn the enemy
                self.enemies[enemyCounter].setPosition(self.getRandomMapPosition())             # Set the enemy position to a random position in the map
            if self.enemies[enemyCounter].isReadyToMove():                                      # Check if the enemy is ready to move
                self.moveEnemy(self.enemies[enemyCounter])                                      # Move the enemy
            if self.enemies[enemyCounter].landed and self.enemies[enemyCounter].isMoving:   # Check if the enemy landed and is moving
                if self.enemyBoard.isInsideBoard(self.enemies[enemyCounter].getPosition()):     # Check if the position is inside the board
                    self.enemies[enemyCounter].land()                                           # Land the enemy
                else:
                    self.enemies[enemyCounter].dropOff()                                        # Make the enemu fall
    
    def checkCollisions(self):
        '''
        Checks the collisions.
        @param self The current object
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        if not self.player.isMoving and self.enemyBoard.getValue(self.player.getPosition()):                          # Check the player is not moving and the board is occupied 
            self.player.hit()                                                                                           # Hit the player
            self.player.loseLives()                                                                                     # Make the player lose lives
            for enemyCounter in range(len(self.enemies)):                                                               # Loop through the enemies
                if self.enemies[enemyCounter].getPosition().x == 0 and self.enemies[enemyCounter].getPosition().y == 0: # Check if the enemy is at the player's position
                    self.enemies[enemyCounter].hit()                                                                    # Hit the enemy
        
    def drawPlayer(self, screen, before):
        '''
        Draws the player.
        @param self The current object
        @param screen The screen.
        @param before A variable that tells if the method was called before the block rendering.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        if before and self.player.isFalling:                                  # If drawn before and the player is falling
            self.player.draw(screen, self.getGraphicPosition(self.player))      # Draw the player
        elif not before and not self.player.isFalling:                        # If after and the player is not falling
            self.player.draw(screen, self.getGraphicPosition(self.player))      # Draw the player
            
    def drawEnemies(self, screen, before):
        '''
        Draws the enemies.
        @param self The current object
        @param screen The screen.
        @param before A variable that tells if the method was called before the block rendering.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        for enemyCounter in range(len(self.enemies)):                                                               # Loop through the enemies
            if self.enemies[enemyCounter].canBeDrawn():                                                             #  Check if the enemy can be drawn
                if before and self.enemies[enemyCounter].isFalling:                                               # If drawn before and the player is falling
                    self.enemies[enemyCounter].draw(screen, self.getGraphicPosition(self.enemies[enemyCounter]))    # Draw the player
                elif not before and not self.enemies[enemyCounter].isFalling:                                     # If after and the player is not falling
                    self.enemies[enemyCounter].draw(screen, self.getGraphicPosition(self.enemies[enemyCounter]))    # Draw the player
                    
    def getGraphicPosition(self, being):
        '''
        Converts a board position into graphical position.
        @param self The current object
        @param being The being to draw.
        @return The position in graphical coordinates.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        location = Math.Vector2(QbertLevel.initialXG + (85 * being.getPosition().y) - (85 * being.getPosition().x) + (85 * being.getJumpMovement().y) - (85 * being.getJumpMovement().x),   # Calculate the new vector with the graphical coordinates
                    QbertLevel.initialYG + (110 * being.getPosition().y) + (110 * being.getPosition().x) + (110 * being.getJumpMovement().y) + (110 * being.getJumpMovement().x))
        return location     # Return the calculated Vector2
        
    def getRandomMapPosition(self):
        '''
        Gets a random map position.
        @param self The current object
        @return A Vector2 with a random valid position in the map.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        x = random.randint(0,self.enemyBoard.getWidth() - 1)            # Get a random x position
        y = random.randint(0,self.clearBoard.getWidth() - x - 1)        # Get a random y position
        return Math.Vector2(x, y)                                   # Create the vector and return it.
