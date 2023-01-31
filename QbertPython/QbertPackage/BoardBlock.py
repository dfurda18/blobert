'''
This class represents a graphical block object for the gameplay in qbert.

@author: Dario Urdapilleta
@version 1.0
@since: 13 nov. 2022
'''
import pygame

class BoardBlock(object):
    '''
    Variables:
    verticesFront: The front square vertexes
    verticesBack: The back square vertexes
    verticesTop: The top square vertexes
    '''

    def __init__(self, location):
        '''
        BoardBlock constructor given its location.
        @param self This object
        @param location The location of this instance.
        @return A new instance of the BoardBlock class.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        self.verticesFront = ((80 + location.x, 20 + location.y),      # The front square vertexes
                              (80 + location.x, 100 + location.y),
                              (0 + location.x, 80 + location.y),
                              (0 + location.x, 0 + location.y))
        self.verticesBack = ((165 + location.x, 0 + location.y),      # The back square vertexes
                              (85 + location.x, 20 + location.y),
                              (85 + location.x, 100 + location.y),
                              (165 + location.x, 80 + location.y))
        self.verticesTop = ((83 + location.x, 16 + location.y),      # The top square vertexes
                              (165 + location.x, -4 + location.y),
                              (83 + location.x, -25 + location.y),
                              (0 + location.x, -4 + location.y))
    
    def draw(self, screen, active):
        '''
        Draws the block.
        @param self This object
        @param screen The game screen.
        @param active If the block should be on or off.

        @author: Dario Urdapilleta
        @version 1.0
        @since 13 nov. 2022
        '''
        pygame.draw.polygon(screen, (222, 173, 190), self.verticesFront)    # Draw the front square
        pygame.draw.polygon(screen, (102, 119, 136), self.verticesBack)     # Draw the back square
        if active:                                                          # Check if the cube is active
            pygame.draw.polygon(screen, (34, 255, 136), self.verticesTop)   # Draw the top square
        else:
            pygame.draw.polygon(screen, (255, 34, 136), self.verticesTop)   # Draw the top square