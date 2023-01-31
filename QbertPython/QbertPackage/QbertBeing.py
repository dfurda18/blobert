'''
This class represents A Qbert being such as the player or enemies.

@author: Dario Urdapilleta
@version 1.0
@since: 13 nov. 2022
'''
import pygame
from enum import Enum
import pygame.math as Math

from QbertPackage import SpriteAnimation

class State(Enum):      # Enum with all the animation states
    IDDLE = 0
    JUMPING_FRONT_UP_RIGHT = 1
    JUMPING_FRONT_UP_LEFT = 2
    JUMPING_FRONT_DOWN_RIGHT = 3
    JUMPING_FRONT_DOWN_LEFT = 4
    JUMPING_BACK_UP_LEFT = 5
    JUMPING_BACK_UP_RIGHT = 6
    JUMPING_BACK_DOWN_LEFT = 7
    JUMPING_BACK_DOWN_RIGHT = 8

class QbertBeingClass(object):
    '''
    Variables:
    maxHeight: The maximum jumping height
    deltaHeight: The height change rate
    deltaMovement: The movement animation frame rate
    deltaJump: The jump animation frame rate
    position: The being's position
    texture: The being's sprite list
    movementSlice: The direction it is moving
    jumpMovement: The actual placement of the being
    animations: The animations
    state: The being's current state
    stateTime: Time for the key frames
    height: The jump height
    isMoving: True if the being is moving
    landed: True if it has landed
    isFalling: True if it's outside of the board
    pleaseRespawn: True if it needs to respawn
    '''
    maxHeight = 100         # The maximum jumping height
    deltaHeight = 4         # The height change rate
    deltaMovement = 0.02    # The movement animation frame rate
    deltaJump = 0.1         # The jump animation frame rate

    def __init__(self, texture, start, animationLength):
        '''
        Creates a new instance of a QbertBeing given the spritesheet information.
        @param self The current object
        @param fileName The Spritesheet's file name.
        @param start The start spritesheet position.
        @param animationLength The length of each animation.
        @return A new instance of the QbertBeing
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.state = State.IDDLE                                                            # Set the start state to IDDLE
        self.isMoving = False                                                               # It starts not moving
        self.landed = True                                                                  # It starts on the ground
        self.isFalling = False                                                              # It starts in the board
        self.pleaseRespawn = False                                                          # It doesn't need to be respawned
        self.movementSlice = Math.Vector2(0, 0)                                             # It's not moving
        self.jumpMovement = Math.Vector2(0, 0)                                              # It's not moving
        self.texture = texture.convert_alpha()                                              # Loads the being's texture
        self.animations = [None] * 5                                                        # Create the animations array
        for animationCounter in range(len(self.animations)):                                # Loop through the animations
            self.animations[animationCounter] = SpriteAnimation.SpriteAnimation(self.texture, animationLength, 60, 60, start + animationCounter)      # Create the animation
            self.animations[animationCounter].isLooping = True                              # Set it to loop
            self.animations[animationCounter].setFramesPersecond(8)                         # Set the frames per second
        self.stateTime = 0                                                                  # Reset the state time
        self.height = 0                                                                     # Set the height as 0
        
    def needsRespawn(self):
        '''
        Returns true if the being needs to respawn.
        @param self The current object
        @return True if it needs to be respwned.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.pleaseRespawn       # Please respawn this being
        
    def isStanding(self):
        '''
        Returns true if the being is standing.
        @param self The current object
        @return True if the being is standing.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.state == State.IDDLE   # Make sure the position is not null, and the state is IDDLE
    
    def isMoving(self):
        '''
        Returns true if the being is moving.
        @param self The current object
        @return True if the being is moving.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.isMoving       # Return if the being is moving
    
    def landed(self):
        '''
        Returns true if the being has landed.
        @param self The current object
        @return True if the being has landed.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.landed         # Return true if the bieng has landed
    
    def isFalling(self):
        '''
        Returns true if the being is falling.
        @param self The current object
        @return True if the being is falling.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.isFalling      # Return true if the being is falling
    
    def setHeight(self, height):
        '''
        Sets the being's height
        @param self The current object
        @param height The being's height.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.height = height       # Set the height
        
    def getJumpMovement(self):
        '''
        Returns the being's actual movement
        @param self The current object
        @return The being's actual movement
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.jumpMovement       # return the actual movement
    
    def getPosition(self):
        '''
        Returns the being's position as a Vector2
        @param self The current object
        @return The being's position as a Vector2
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        return self.position       # Return the position
    
    def setPosition(self, position):
        '''
        Sets the being's position
        @param self The current object
        @param position The being's new position.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.position = position   # Set the position
        
    def move(self, direction):
        '''
        Moves the being torwards a direction.
        @param self The current object
        @param direction The direction the being is moving.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.isMoving = True                                        # Set isMoving to true
        self.landed = False                                         # Set landed to false
        if direction == 0:                                          # north west
            self.movementSlice.y = -QbertBeingClass.deltaMovement        # Move negative on y
            self.movementSlice.x = 0                                # Leave x unchanged
            self.state = State.JUMPING_BACK_UP_LEFT                 # Change state to JUMPING_BACK_UP_LEFT
        elif direction == 1:                                        # north east
            self.movementSlice.x = -QbertBeingClass.deltaMovement        # Move negative on x
            self.movementSlice.y = 0                                # Leave y unchanged
            self.state = State.JUMPING_BACK_UP_RIGHT                # Change state to JUMPING_BACK_UP_RIGHT
        elif direction == 2:                                        # north east
            self.movementSlice.y = QbertBeingClass.deltaMovement         # Move positive on y
            self.movementSlice.x = 0                                # Leave x unchanged
            self.state = State.JUMPING_FRONT_UP_RIGHT               # Change state to JUMPING_FRONT_UP_RIGHT
        else:
            self.movementSlice.x = QbertBeingClass.deltaMovement         # Move positive on x
            self.movementSlice.y = 0                                # Leave y unchanged
            self.state = State.JUMPING_FRONT_UP_LEFT                # Change state to JUMPING_FRONT_UP_LEFT
        self.stateTime = 0                                          # Reset the state time
        
    def update(self, gameTime):
        '''
        Updates the being
        @param self The current object
        @param gameTime The game time transcurred since last update.
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.stateTime = self.stateTime + gameTime                  # Increase the animation state
        if self.state == State.IDDLE:                               # When it is IDDLE
            self.height = 0                                         # Set the height to 0
        elif self.state == State.JUMPING_FRONT_UP_RIGHT:            # When it is JUMPING_FRONT_UP_RIGHT
            self.height = self.height + QbertBeingClass.deltaHeight      # Increase the height
        elif self.state == State.JUMPING_FRONT_UP_LEFT:             # When it is JUMPING_FRONT_UP_LEFT
            self.height = self.height + QbertBeingClass.deltaHeight      # Increase the height
        elif self.state == State.JUMPING_FRONT_DOWN_RIGHT:          # When it is JUMPING_FRONT_DOWN_RIGHT
            self.height = self.height - QbertBeingClass.deltaHeight      # Decrease the height
        elif self.state == State.JUMPING_FRONT_DOWN_LEFT:           # When it is JUMPING_FRONT_DOWN_LEFT
            self.height = self.height - QbertBeingClass.deltaHeight      # Decrease the height
        elif self.state == State.JUMPING_BACK_UP_LEFT:              # When it is JUMPING_BACK_UP_LEFT
            self.height = self.height + QbertBeingClass.deltaHeight      # Increase the height
        elif self.state == State.JUMPING_BACK_UP_RIGHT:             # When it is JUMPING_BACK_UP_RIGHT
            self.height = self.height + QbertBeingClass.deltaHeight      # Increase the height
        elif self.state == State.JUMPING_BACK_DOWN_LEFT:            # When it is JUMPING_BACK_DOWN_LEFT
            self.height = self.height - QbertBeingClass.deltaHeight      # Decrease the height
        elif self.state == State.JUMPING_BACK_DOWN_RIGHT:           # When it is JUMPING_BACK_DOWN_RIGHT
            self.height = self.height - QbertBeingClass.deltaHeight      # Decrease the height
        else:
            self.height = 0                                         # Set the height to 0
        if self.height >= QbertBeingClass.maxHeight:                     # If the being has reached the max height
            if self.state == State.JUMPING_FRONT_UP_RIGHT:          # When it is JUMPING_FRONT_UP_RIGHT
                self.state = State.JUMPING_FRONT_DOWN_RIGHT         # Change to JUMPING_FRONT_DOWN_RIGHT
            elif self.state == State.JUMPING_FRONT_UP_LEFT:         # When it is JUMPING_FRONT_UP_LEFT
                self.state = State.JUMPING_FRONT_DOWN_LEFT          # Change to JUMPING_FRONT_DOWN_LEFT
            elif self.state == State.JUMPING_BACK_UP_LEFT:          # When it is JUMPING_BACK_UP_LEFT
                self.state = State.JUMPING_BACK_DOWN_LEFT           # Change to JUMPING_BACK_DOWN_LEFT
            elif self.state == State.JUMPING_BACK_UP_RIGHT:         # When it is JUMPING_BACK_UP_RIGHT
                self.state = State.JUMPING_BACK_DOWN_RIGHT          # Change to JUMPING_BACK_DOWN_RIGHT
        self.jumpMovement.x = self.jumpMovement.x + self.movementSlice.x    # Move the being on the x axis
        self.jumpMovement.y = self.jumpMovement.y + self.movementSlice.y    # Move the being on the y axis
        if not self.isFalling and self.height < 0:                  # Check if it's not falling and it has landed
            if self.jumpMovement.x >= 1:                            # Check the movement on x is more than 1
                self.position.x = self.position.x + 1               # Increase the position in x
            if self.jumpMovement.x <= -1:                           # Check the movement on x is lees than -1
                self.position.x = self.position.x - 1               # Decrease the position in x
            if self.jumpMovement.y >= 1:                            # Check the movement on y is more than 1
                self.position.y = self.position.y + 1               # Increase the position in y
            if self.jumpMovement.y <= -1:                           # Check the movement on y is lees than -1
                self.position.y = self.position.y - 1               # Decrease the position in y
            self.landed = True                                      # Land the being
        if self.height <= -300:                                     # Check if the height has gone less than -300
            self.pleaseRespawn = True                               # Request to respawn
        for animationCounter in range(len(self.animations)):        # Loop through the animations
            self.animations[animationCounter].update(gameTime)      # Update the animation
            
    def draw(self, screen, graphicPosition):
        '''
        Draws the being
        @param self The current object
        @param screen The screen.
        @param graphicPosition The being's acutal position
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        for animationCounter in range(len(self.animations)):                # Loop through the animations
            self.animations[animationCounter].position = Math.Vector2(graphicPosition.x, graphicPosition.y - self.height)    # Update the animations position
        if self.state == State.IDDLE:                               # When it is IDDLE
            self.animations[0].draw(screen, self.animations[0].position)        # Show the idle animation
        elif self.state == State.JUMPING_FRONT_UP_RIGHT:            # When it is JUMPING_FRONT_UP_RIGHT
            self.animations[1].draw(screen, self.animations[0].position)        # Show the JUMPING_FRONT_UP_RIGHT animation
        elif self.state == State.JUMPING_FRONT_UP_LEFT:             # When it is JUMPING_FRONT_UP_LEFT
            self.animations[2].draw(screen, self.animations[0].position)        # Show the JUMPING_FRONT_UP_LEFT animation
        elif self.state == State.JUMPING_FRONT_DOWN_RIGHT:          # When it is JUMPING_FRONT_DOWN_RIGHT
            self.animations[1].draw(screen, self.animations[0].position)        # Show the JUMPING_FRONT_DOWN_RIGHT animation
        elif self.state == State.JUMPING_FRONT_DOWN_LEFT:           # When it is JUMPING_FRONT_DOWN_LEFT
            self.animations[2].draw(screen, self.animations[0].position)        # Show the JUMPING_FRONT_DOWN_LEFT animation
        elif self.state == State.JUMPING_BACK_UP_LEFT:              # When it is JUMPING_BACK_UP_LEFT
            self.animations[4].draw(screen, self.animations[0].position)        # Show the JUMPING_BACK_UP_LEFT animation
        elif self.state == State.JUMPING_BACK_UP_RIGHT:             # When it is JUMPING_BACK_UP_RIGHT
            self.animations[3].draw(screen, self.animations[0].position)        # Show the JUMPING_BACK_UP_RIGHT animation
        elif self.state == State.JUMPING_BACK_DOWN_LEFT:            # When it is JUMPING_BACK_DOWN_LEFT
            self.animations[4].draw(screen, self.animations[0].position)        # Show the JUMPING_BACK_DOWN_LEFT animation
        elif self.state == State.JUMPING_BACK_DOWN_RIGHT:           # When it is JUMPING_BACK_DOWN_RIGHT
            self.animations[3].draw(screen, self.animations[0].position)        # Show the JUMPING_BACK_DOWN_RIGHT animation
        else:
            self.animations[0].draw(screen, graphicPosition)        # Show the idle animation
            
    def hit(self):
        '''
        Hits the being.
        @param self The current object
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.pleaseRespawn = True      # Request respawn
        self.isMoving = True           # Set moving to true
        self.resetValues()             # Reset the values
        
    def resetValues(self):
        '''
        Resets the values for a new respawn.
        @param self The current object
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.jumpMovement.x = 0       # Movement x to 0
        self.movementSlice.x = 0      # Movement slice x to 0
        self.jumpMovement.y = 0       # Movement y to 0
        self.movementSlice.y = 0      # Movement slice y to 0
        
    def dropOff(self):
        '''
        Sets the being to fall definitely.
        @param self The current object
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.resetValues()             # Reset the values
        self.isFalling = True          # Set falling to true
        self.landed = False            # Set landed to false
        
    def land(self):
        '''
        Lands the being.
        @param self The current object
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        if self.height <= 0:            # Check if the height is less than 0
            self.height = 0             # Set the height to 0
            self.state = State.IDDLE    # Set the state to IDDLE
        self.resetValues()              # Reset the values
        self.isMoving = False           # Set isMoving to false
        
    def respawn(self):
        '''
        Respawns the being.
        @param self The current object
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.setHeight(QbertBeingClass.maxHeight)                   # Set the height to the max
        self.state = State.JUMPING_FRONT_DOWN_RIGHT                 # Change the state to     JUMPING_FRONT_DOWN_RIGHT
        self.landed = False                                         # Set landed to false
        self.isMoving = True                                        # Set isMoving to true
        self.isFalling = False                                      # Set is Falling to false
        self.pleaseRespawn = False                                  # Request to respawn
        self.stateTime = 0.5                                        # Set the state time to 0.5