'''
Class that Animates a sprite list.

@author: Dario Urdapilleta
@version 1.0
@since: 13 nov. 2022
'''
import pygame

class SpriteAnimation(object):
    '''
    Variables:
    frameIndex: The current frame
    isLooping: Value to know if the animation loops
    timeToUpdate: Time to update
    timeElapsed: The tie elapsed since last update
    image: The texture
    rectangles: The Rectangles
    position: The position
    '''

    def __init__(self, image, frames, spriteWidth, spriteHeight, start):
        '''
        Creates a new instance of a SpriteAnimation given the texture and number of frames.
        @param self The current object
        @param image The animation texture
        @param frames The number of frames
        @param spriteWidth The sprite width
        @param spriteHeight The sprite height
        @para, start The starting line
        @return A new instance of the SpriteAnimation class.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.frameIndex = 0                                                                                     # Set the frame index to 0
        self.isLooping = False                                                                                  # Set isLooping to False
        self.timeToUpdate = 1/20                                                                                # Set the timeToupdate to 1/20
        self.timeElapsed = 0                                                                                    # Set the timeElapsed to 0
        self.image = image.convert_alpha()                                                                      # Loads the image
        self.rectangles = [None] * frames                                                                       # Create the rectangles
        for frameCounter in range(frames):                                                                      # Loop through the rectangles
            self.rectangles[frameCounter] = pygame.Surface((spriteWidth, spriteHeight), pygame.SRCALPHA, 32).convert_alpha()         # Create the Rectangle
            self.rectangles[frameCounter].blit(self.image, (0,0),  # Blit the image into the rectangle
                            (spriteWidth * frameCounter, start * spriteHeight, spriteWidth, spriteHeight))
        
    def setFramesPersecond(self, framesPerSecond):
        '''
        Sets the frames per second
        @param self The current object
        @param Frames per second
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.timeToUpdate = 1 / framesPerSecond     # Set the time to update
        
    def draw(self, screen, position):
        '''
        Sets the frames per second
        @param self The current object
        @param Frames per second
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        screen.blit(self.rectangles[self.frameIndex], (position.x, position.y))       # Display the frame
        
    def update(self, time):
        '''
        Updates the sprite animation
        @param self The current object
         
        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.timeElapsed = self.timeElapsed + time                      # Add to the elapsed time the time passed since last update
        if self.timeElapsed > self.timeToUpdate:                        # Check if it is time to update
            self.timeElapsed = self.timeElapsed - self.timeToUpdate     # Reduce the time to update
            if self.frameIndex < len(self.rectangles) - 1:              # Check if there's still more frames to animate
                self.frameIndex = self.frameIndex + 1                   # Increase the frame
            elif self.isLooping:                                        # Check if it's the last one and it should loop
                self.frameIndex = 0                                     # Loop the animation