'''
Created on 12 nov. 2022

This class represents The QbertPackage game.

@author: Dario Urdapilleta
@version 1.0
@since: 12 nov. 2022
'''
import pygame
import os
from QbertPackage import QbertLevel
from QbertPackage import QbertPlayer
from QbertPackage import PlayerRecord
from enum import Enum
import time
from pickle import FALSE

class GameState(Enum):      # All the different game states
    TITLE_SCREEN = 0
    GAMEPLAY = 1
    PAUSE = 2
    GAME_OVER = 3
    SCORE_TABLE = 4
    NEXT_LEVEL = 5
    
class Qbert():
    '''
    Variables:
    MAX_DISPLAY_SCORE: The max amount of records to display
    LIFE_BONUS: The amount of score needed to gain a new life
    TITLE_OPTIONS: The number of menu options in the title screen
    NW: Constant for the North West Direction
    NE: Constant for the North East Direction
    SE: Constant for the South East Direction
    SW: Constant for the South West Direction
    highScores: The list of highScores
    previousLivesInrement: The last time a used received a bonus
    initials: The latest initials used
    menuSelection: The element selected in the menu
    buttonIsPressed: A boolean that tells if the user pressed a button
    gameState: The current game state
    lastTime: The last time the game was updated
    background: The background texture
    title: The title texture
    playerTexture: The characters texture
    font: The game's font
    level: The current level number
    score: The current score used to keep track between levels
    initialSelected: The position of the initials selected
    currentLevel: The current level object
    player: The player
    '''
    MAX_DISPLAY_SCORE = 8                              # The max amount of records to display
    LIFE_BONUS = 1000                                  # The amount of score needed to gain a new life
    TITLE_OPTIONS = 3                                  # The number of menu options in the title screen
    NW = 0                                             # Constant for the North West Direction
    NE = 1                                             # Constant for the North East Direction
    SE = 2                                             # Constant for the South East Direction
    SW = 3                                             # Constant for the South West Direction
    
    def __init__(self):
        '''
        The QbertPackage constructor.
        
        @param self The current object
        @return A new instance of the QbertPackage class.
        
        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.highScores = []                                # Set initial value to the scores
        self.previousLivesInrement = 0                      # Sets the previous lives increment to 0
        self.initials = ['A', 'A', 'A']                     # Create the initials
        self.loadScores()                                   # Loads the scores from the file
        self.menuSelection = 0                              # Sets the initial value of the menu selection to 0
        self.buttonIsPressed = False                        # Sets the initial value of a button pressed to false
        self.gameState = GameState.TITLE_SCREEN             # Sets the initial game state to TITLE_SCREEN
        self.lastTime = time.time()                         # Set the last time
        self.background = pygame.image.load("Qbert.png")    # Loads the background texture
        self.title = pygame.image.load("Title.png")         # Loads the title texture
        self.playerTexture = pygame.image.load("AnimationSpritelist.png") # Loads the player texture
        self.font = pygame.font.Font('SyneMono-Regular.ttf', 60)    # Laod the font
        
          
    def input(self):
        '''
        Handles the keyboard input.
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        for event in pygame.event.get():                                        # Loop on the events
            if event.type == pygame.QUIT:                                       # Check is the event request to quit
                pygame.quit()                                                   # Quit Pygame
                exit()                                                          # Quit the application
            elif event.type == pygame.KEYDOWN:                                  # Check if a key was pressed
                key = event.key                                                 # Get the key
                if key == pygame.K_ESCAPE:                                      # Check if the key is ESCAPE
                    pygame.quit()                                               # Quit Pygame
                    exit()                                                      # Quit the application
                if self.gameState == GameState.TITLE_SCREEN:                    # For the TITLE_SCREEN
                    if not self.buttonIsPressed:                                # Make sure no button is pressed
                        if key == pygame.K_UP:                                  # Handle if the UP key is pressed
                            if self.menuSelection > 0:                          # Make sure the selected element is greater than 0
                                self.buttonIsPressed = True                     # Let the class know a button was pressed
                                self.menuSelection = self.menuSelection - 1     # Reduce the menu selection by 1
                        elif key == pygame.K_DOWN:                              # Handle the DOWN key
                            if self.menuSelection < self.TITLE_OPTIONS - 1:     # Make sure the selected element is greater than 0
                                self.buttonIsPressed = True                     # Let the class know a button was pressed
                                self.menuSelection = self.menuSelection + 1     # Increment the menu selection
                        elif key == pygame.K_RETURN:                            # Handle pressing the ENTER key
                            self.buttonIsPressed = True                         # Let the class know a button was pressed
                            if self.menuSelection == 0:                         # Start game was selected
                                self.level = 1                                  # Set the initial level to 1
                                self.score = 0                                  # Set the initial score to 0
                                self.player = QbertPlayer.QbertPlayer(self.playerTexture)  # Create a new Player
                                self.loadLevel()                                # Load a new level
                                self.gameState = GameState.GAMEPLAY             # Change the game state to GAMEPLAY
                            elif self.menuSelection == 1:                       # High score option was selected
                                self.gameState = GameState.SCORE_TABLE          # Change the game state to SCORE_TABLE
                            elif self.menuSelection == 2:                       # High score option was selected
                                pygame.quit()                                   # Quit Pygame
                                exit()                                          # Quit the application
                elif self.gameState == GameState.SCORE_TABLE:                   # For the SCORE_TABLE
                    if not self.buttonIsPressed:                                # Make sure no button is pressed
                        if key == pygame.K_RETURN:                              # Handle pressing the ENTER key
                            self.gameState = GameState.TITLE_SCREEN;            # Return to the TITLE_SCREEN
                            self.buttonIsPressed = True                         # Let the class know a button was pressed
                elif self.gameState == GameState.GAME_OVER:                     # Handle input in the GAME_OVER screen
                    if not self.buttonIsPressed:                                # Make sure no button is pressed
                        if key == pygame.K_RETURN:                              # Handle pressing the ENTER key
                            self.saveScore(self.initials, self.currentLevel.getCurrentScore())      # Save the current score in the score list and file
                            self.gameState = GameState.TITLE_SCREEN             # Change the game state to the TITLE_SCREEN
                            self.buttonIsPressed = True                         # Let the class know a button was pressed
                        elif key == pygame.K_LEFT:                              # Handle pressing the LEFT key
                            if self.initialSelected > 0:                       # Handle pressing the LEFT key
                                self.buttonIsPressed = True                     # Let the class know a button was pressed
                                self.initialSelected = self.initialSelected - 1 # Reduce the selected initial by 1
                        elif key == pygame.K_RIGHT:                             # Handle pressing the RIGHT button
                            if self.initialSelected < 2:                        # Handle pressing the LEFT key
                                self.buttonIsPressed = True                     # Let the class know a button was pressed
                                self.initialSelected = self.initialSelected + 1 # Increment the initial selected
                        elif key == pygame.K_UP:                                # Handle if the UP key is pressed
                            if self.initials[self.initialSelected] >= 'Z':      # Check if the current letter is Z or more
                                self.initials[self.initialSelected] = 'A'       # Change it to A
                            else:
                                self.initials[self.initialSelected] = chr(ord(self.initials[self.initialSelected])+1)        # Otherwise, increment it by 1
                                self.buttonIsPressed = True                     # Let the class know a button was pressed
                        elif key == pygame.K_DOWN:                              # Handle the DOWN key
                            if self.initials[self.initialSelected] <= 'A':      # Check if the current is A or lower
                                self.initials[self.initialSelected] = 'Z'       # Set it to Z
                            else:
                                self.initials[self.initialSelected] = chr(ord(self.initials[self.initialSelected])-1)   # Decrease the letter by 1
                                self.buttonIsPressed = True                     # Let the class know a button was pressed
                elif self.gameState == GameState.GAMEPLAY:                      # Handle input in the GAMEPLAY screen
                    if not self.buttonIsPressed:                                # Make sure no button is pressed
                        if not self.player.isMoving:                            # Make sure the player is not moving
                            if key == pygame.K_q:                               # Handle pressing the Q key
                                self.currentLevel.movePlayer(Qbert.NW)          # Move the player North West
                            elif key == pygame.K_w:                             # Handle pressing the W key
                                self.currentLevel.movePlayer(Qbert.NE)          # Move the player North East
                            elif key == pygame.K_s:                             # Handle pressing the S key
                                self.currentLevel.movePlayer(Qbert.SE)          # Move the player South East
                            elif key == pygame.K_a:                             # Handle pressing the W key
                                self.currentLevel.movePlayer(Qbert.SW)          # Move the player South West
                        if key == pygame.K_p:                                   # Handle pressing the P key
                            self.gameState = GameState.PAUSE;                   # Change the game state to GAMEPLAY
                            self.buttonIsPressed = True                         # Let the class know a button was pressed
                elif self.gameState == GameState.NEXT_LEVEL:                    # Handle the input in the NEXT_LEVEL screen
                    if not self.buttonIsPressed:                                # Make sure no button is pressed
                        if key == pygame.K_RETURN:                              # Handle pressing the ENTER key
                            self.score = self.currentLevel.getCurrentScore()    # Update the score with the previous level score
                            self.level = self.level + 1                         # Increment the level
                            self.loadLevel()                                    # Load a new Level
                            self.gameState = GameState.GAMEPLAY                 # Change the game state to GAMEPLAY
                elif self.gameState == GameState.PAUSE:                         # Handle when the input on the PAUSE screen
                    if not self.buttonIsPressed:                                # Make sure no button is pressed
                        if key == pygame.K_p:                                   # Handle pressing the P key
                            self.gameState = GameState.GAMEPLAY;                # Change the game state to GAMEPLAY
                            self.buttonIsPressed = True                         # Let the class know a button was pressed
        if  sum(pygame.key.get_pressed()) == 0:                                 # Make sure no important key is pressed
            self.buttonIsPressed = False                                        # Let the class know no button is pressed
    
    def update(self, gameTime):
        '''
        Draws for the camera.
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        if self.gameState == GameState.GAMEPLAY:                                # When the game state is GAMEPLAY
            self.currentLevel.update(gameTime)                                  # Update the current level
            if self.currentLevel.getCurrentScore() - self.previousLivesInrement > Qbert.LIFE_BONUS:      # If the player's score has gone above the life bonus
                self.player.oneUp()                                             # Add one life to the player
                self.previousLivesInrement = self.currentLevel.getCurrentScore()       # Update the next life increment step
            if self.currentLevel.gameCompleted():                               # If the game is completed
                self.gameState = GameState.NEXT_LEVEL                           # Change the state to the NEXT_LEVEL
            if self.player.isDead():                                            # if the player is dead
                self.initialSelected = 0                                        # Set the initial selected to the first letter
                self.gameState = GameState.GAME_OVER                            # Change the game state to the GAME_OVER state

    def draw(self):
        '''
        Draws for the camera.
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        screen.fill((0, 0, 0))                                                  # Clear the screen
        screen.blit(self.background, self.background.get_rect())                # Render the Background for all states
        if self.gameState == GameState.TITLE_SCREEN:                            # For the TITLE_SCREEN 
            backSquare = pygame.Surface((405,290), pygame.SRCALPHA)             # Create a square
            backSquare.fill((0,0,0,200))                                        # Set the color with alpha
            screen.blit(backSquare, (750,560))                                  # Display the square
            backSquare = pygame.Surface((405,90), pygame.SRCALPHA)              # Create a square for the selection
            backSquare.fill((120,100,30,220))                                   # Set the color with alpha
            screen.blit(backSquare, (750,560 + (self.menuSelection * 100)))     # Display the square
            screen.blit(self.title, (420,200))                                  # Render the Background for all states
            text = self.font.render('New Game', True, (255,255,255))            # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (960, 600)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            text = self.font.render('High Score', True, (255,255,255))          # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (960, 700)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            text = self.font.render('Quit Game', True, (255,255,255))           # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (960, 800)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
        elif self.gameState == GameState.SCORE_TABLE:                           # For the SCORE_TABLE
            backSquare = pygame.Surface((1920,1080), pygame.SRCALPHA)           # Create a square
            backSquare.fill((0,0,0,200))                                        # Set the color with alpha
            screen.blit(backSquare, (0,0))                                      # Display the square
            text = self.font.render('HIGH SCORES', True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 100)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            recordsDisplayed = 0                                                # Variable to count the records displayed
            for scoreCounter in self.highScores:                                # Loop through the highscores
                if(recordsDisplayed < Qbert.MAX_DISPLAY_SCORE):                # Make sure there's room for this record
                    text = self.font.render(scoreCounter.getName() + " " + str(scoreCounter.getScore()), True, (255,255,255))         # Get the text
                    textRect = text.get_rect()                                  # Get the text rectangle
                    textRect.center = (980, 200 + (recordsDisplayed * 100))     # Get the rect center
                    screen.blit(text, textRect)                                 # Blit the text
                recordsDisplayed = recordsDisplayed + 1                         # Increase the record displayed
            text = self.font.render('Press ENTER to return to the Title Screen.', True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 980)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
        elif self.gameState == GameState.NEXT_LEVEL:                            # Draw the NEXT_LEVEL
            backSquare = pygame.Surface((910,310), pygame.SRCALPHA)             # Create a square
            backSquare.fill((0,0,0,200))                                        # Set the color with alpha
            screen.blit(backSquare, (510,370))                                  # Display the square
            text = self.font.render("LEVEL " + str(self.level) + " COMPLETED!", True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 450)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            text = self.font.render("Press ENTER to continue.", True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 610)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
        elif self.gameState == GameState.GAME_OVER:                             # Draw the GAME_OVER
            backSquare = pygame.Surface((1170,510), pygame.SRCALPHA)            # Create a square
            backSquare.fill((0,0,0,200))                                        # Set the color with alpha
            screen.blit(backSquare, (380,300))                                  # Display the square
            backSquare = pygame.Surface((30,60), pygame.SRCALPHA)               # Create a square for the selection
            backSquare.fill((120,100,30,220))                                   # Set the color with alpha
            screen.blit(backSquare, (1179 + self.initialSelected * 34,570))     # Display the square
            text = self.font.render("GAME OVER!", True, (255,255,255))          # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 400)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            text = self.font.render("Your score was: " + str(self.currentLevel.getCurrentScore()), True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 500)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            text = self.font.render("Your initials: "+"".join(self.initials), True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 600)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
            text = self.font.render("Press ENTER to save your score.", True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 700)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
        elif self.gameState == GameState.PAUSE:                                 # Draw the PAUSE
            backSquare = pygame.Surface((1920,1080), pygame.SRCALPHA)           # Create a square
            backSquare.fill((0,0,0,200))                                        # Set the color with alpha
            screen.blit(backSquare, (0,0))                                      # Display the square
            text = self.font.render("PAUSED", True, (255,255,255))         # Get the text
            textRect = text.get_rect()                                          # Get the text rectangle
            textRect.center = (980, 580)                                        # Get the rect center
            screen.blit(text, textRect)                                         # Blit the text
        elif self.gameState == GameState.GAMEPLAY:                              # Draw the GAMEPLAY
            backSquare = pygame.Surface((700,210), pygame.SRCALPHA)             # Create a square
            backSquare.fill((0,0,0,200))                                        # Set the color with alpha
            screen.blit(backSquare, (20,30))                                    # Display the square
            self.currentLevel.drawPlayer(screen, True)                          # Render the player only if it is falling behind
            self.currentLevel.drawEnemies(screen, True)                         # Render the enemies falling behind
            self.currentLevel.drawBlocks(screen)                                # Draw the platforms
            self.currentLevel.drawPlayer(screen, False)                         # Render the player if it is in fron of the platforms
            self.currentLevel.drawEnemies(screen, False)                        # Render the enemies that are in fron of the platforms
            text = self.font.render("Score: " + str(self.currentLevel.getCurrentScore()), True, (255,255,255))         # Get the text
            screen.blit(text, (50,30))                                          # Blit the text
            text = self.font.render("Level: " + str(self.level), True, (255,255,255))         # Get the text
            screen.blit(text, (50,90))                                          # Blit the text
            text = self.font.render("Lives: " + str(self.currentLevel.playersLives()), True, (255,255,255))         # Get the text
            screen.blit(text, (50,150))                                         # Blit the text
        pygame.display.flip()                                                   # Flip the display
        
    def loadLevel(self):
        '''
        Loads a level depending on the current level number.
        @param self The current object

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.currentLevel = QbertLevel.QbertLevel(self.level, self.player, self.score, self.playerTexture);         # Create a new level
    def loadScores(self):
        '''
        Loads the scores from a binary file.

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        if os.path.isfile("scores.dat"):                                                # Check if the file exists
            file = open("scores.dat", "rb")                                             # Open the file to read bytes
            allBytes = file.read()                                                      # Read all the bytes
            file.close()                                                                # Close the file
            name = ['A', 'A', 'A']                                                      # Declare a list to store the name
            score = [None] * 4                                                          # Declare a list to store the score
            for bytesCounter in range(0, len(allBytes), 7):                             # Loop through the bytes 7 at a time
                for nameCounter in range(3):                                            # Loop through the name
                    name[nameCounter] = chr(allBytes[bytesCounter + nameCounter])       # Store the initial to the list
                for scoreCounter in range(4):                                           # Loop through the score
                    score[scoreCounter] = allBytes[bytesCounter + 3 + scoreCounter]     # Store the int information in the list
                self.highScores.append(PlayerRecord.PlayerRecord(name, int.from_bytes(score, byteorder='big', signed=True)))    # Create a Player Record and append it to the highscores
                
    def saveScore(self, name, score):
        '''
        Adds a new score into the list and saves the scores into a file.
        @param self The current object
        @param name The new score initials.
        @param score The new score.

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        self.addScore(PlayerRecord.PlayerRecord(name, score))                                   # Add the score to the score list
        if os.path.isfile("scores.dat"):                                                        # Check if the scores file exists
            os.remove("scores.dat")                                                             # Delete the file
        file = open("scores.dat", "wb")                                                         # Open the file to write in bytes
        savedRecords = 0                                                                        # Declare a variable to keep track of saved records
        for scoreCounter in self.highScores:                                                    # Loop through the highscores
            if savedRecords < Qbert.MAX_DISPLAY_SCORE:                                          # Make sure we don't save more than 10
                file.write(bytes(scoreCounter.getName(), 'ascii'))                              # Write the name
                file.write(scoreCounter.getScore().to_bytes(4, byteorder = 'big', signed=True)) # Write the score
            savedRecords = savedRecords + 1                                                     # Increase the saved records
        file.close()                                                                            # Close the file
                
    def addScore(self, score):
        '''
        Adds a new score into the list.
        @param self The current object
        @param score The new PlayerScore

        @author: Dario Urdapilleta
        @version 1.0
        @since 12 nov. 2022
        '''
        foundRank = False                                   # Declare a false boolean to see if a location for the new score has been found
        rank = 0                                            # Declare an int for the new score's position
        for scoreCounter in self.highScores:                # Loop through the score list until it is over or an adequate rank has been found
            if not foundRank:                                   # Check if rank was not found
                if scoreCounter.getScore() < score.getScore():  # Check if the new score is higher than the current score
                    foundRank = True                            # We have found an appropriate rank
                    self.highScores.insert(rank, score)         # Add the new score before the rank
                else:
                    rank = rank + 1                             # Otherwise increment the position
        if not foundRank:                                   # If no score lower was found
            self.highScores.append(score)                   # Add the score at the end
            
  
pygame.init()                                                       # initializing pygame
  
screen = pygame.display.set_mode((1920, 1080), pygame.FULLSCREEN)   # Sets to Fullscreen
pygame.display.set_caption("Blo*Bert")                              # Set the window name
clock = pygame.time.Clock()                                         # Start the clock
  
qbert = Qbert()                                                     # Create the game object

while True:                                                         # Loop until the game exits
    timeSlice = clock.tick() / 1000                                 # Get the timeSlice
    qbert.input()                                                   # Handle the input
    qbert.update(timeSlice)                                             # Update the game
    qbert.draw()                                                    # Draw the game
    pygame.display.update()                                         # Update pygame