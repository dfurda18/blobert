using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QbertCSharp
{
    /**
     * This class represents A Qbert level that manages all the gameplay.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class QbertLevel
    {
        private const float initialX = 850;     // The graphical initial x position
        private const float initialY = 340;     // The graphical initial y position
        private const float initialXG = 900;    // The graphical initial x position
        private const float initialYG = 280;    // The graphical initial y position
        private QbertBoard clearBoard;          // The game board
        private QbertBoard enemyBoard;          // The enemy board
        private QbertPlayer player;             // The player
        private QbertEnemy[] enemies;           // The enemy list
        private BoardBlock[] blocks;            // The graphical version of the blocks
        private Random random;                  // A random generator
        private short level;                    // The level number
        private int currentScore;               // The current score
        /**
	 * Creates a new QbertLevel given a player and the previous score.
	 * @param level The level number.
	 * @param player The player.
	 * @param score The previous score.
	 * @param graphicsDevice The graphics device
	 * @param enemyTexture The enemies' texture
	 * @return A new instance of the QbertLevel
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
        public QbertLevel(short level, ref QbertPlayer player, int score, GraphicsDevice graphicsDevice, Texture2D enemyTexture)
        {
            this.random = new Random();                                     // Instantiate the random generator
            int numberOfEnemies = (level / 3) + 1;                          // Calculate the number of enemies.
            short lineCounter;                                              // Declare the counter for lines
            short position = 0;                                             // Declare the initial position and set it to 0
            this.level = level;                                             // Set the level number
            this.currentScore = score;                                      // Set the current score
            this.clearBoard = new QbertBoard();                             // Create the game board
            this.enemyBoard = new QbertBoard();                             // Create the enemy board
            this.player = player;                                           // Set the player
            this.blocks = new BoardBlock[clearBoard.GetSize()];             // Create the block array
            for (int boardCounter = 0; boardCounter < this.clearBoard.Size(0); boardCounter++)                              // Loop through the board
            {
                for (lineCounter = 0; lineCounter < this.clearBoard.Size(boardCounter); lineCounter++)                      // Loop through each line
                {
                    blocks[position] = new BoardBlock(this.GetBlockLocation(boardCounter, lineCounter), graphicsDevice);    // Create a new block in the next position
                    position++;                                             // Increase the position counter
                }
            }
            this.enemies = new QbertEnemy[numberOfEnemies];                 // Create the enemy array
            for (int enemyCounter = 0; enemyCounter < this.enemies.Length; enemyCounter++)                                  // Loop through the enemy array
            {
                this.enemies[enemyCounter] = new QbertEnemy(enemyTexture);  // Create a new enemy
                this.enemies[enemyCounter].SetPosition(this.GetRandomMapPosition());                // Set the enemy position to a random position in the map

            }
            this.player.Respawn();                                          // Respawn the player
        }
        /**
         * Disposes of all the graphical elements.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Dispose()
        {
            for (int blockCounter = 0; blockCounter < this.blocks.Length; blockCounter++)       // Loop through the blocks
            {
                this.blocks[blockCounter].Dispose();                                            // Delete each block
            }
            for (int enemyCounter = 0; enemyCounter < this.enemies.Length; enemyCounter++)      // Loop through the enemies
            {
                this.enemies[enemyCounter].Dispose();                                           // Delete the enemy
            }
        }
        /**
         * Returns the current score.
         * @return The current score.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public int GetCurrentScore()
        {
            return this.currentScore;       // Return the current score
        }
        /**
         * Returns if the game level has been completed.
         * @return True if the player has cleared a board.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool GameCompleted()
        {
            return this.clearBoard.IsCompleted();   // Return true if the game board is completed
        }
        /**
         * Moves the player to a direction.
         * @param direction The direction to move the player.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void MovePlayer(int direction)
        {
            if (!this.player.IsMoving())            // Make sure the player is not moving
            {
                this.player.Move(direction);        // Move the player to the direction
            }
        }
        /**
         * Moves the enemy to a random direction.
         * @param enemy The enemy to move.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void MoveEnemy(QbertEnemy enemy)
        {
            int direction = this.random.Next(4);        // Calculate a random direction
            if (!enemy.IsMoving())                      // Make sure the enemy is not moving
            {
                enemy.Move(direction);                  // Move the enemy to the random direction
            }
        }
        /**
         * Converts a board position into a graphical position.
         * @param boardCounter The x position.
         * @param lineCounter The y position
         * @return A Vector2 with the graphical position.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private Vector2 GetBlockLocation(int boardCounter, int lineCounter)
        {
            Vector2 location = new Vector2(QbertLevel.initialX + (85 * lineCounter) - (85 * boardCounter), QbertLevel.initialY + (110 * lineCounter) + (110 * boardCounter));       // Create the vector witht he graphical values
            return location;        // Return the vector
        }
        /**
         * Draws all the blocks
         * @param graphicDevice The graphic device
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void DrawBlocks(GraphicsDevice graphicDevice)
        {
            short position = 0;                          // Declare the iteration for the board array
            short lineCounter;                           // Declare the iterator for the lines
            for (short boardCounter = 0; boardCounter < this.clearBoard.Size(0); boardCounter++)                                // Loop through the lines
            {
                for (lineCounter = 0; lineCounter < this.clearBoard.Size(boardCounter); lineCounter++)
                {                           // Loop through the cells of each line
                    this.blocks[position].Draw(graphicDevice, this.clearBoard.GetValue(new Vector2(boardCounter, lineCounter)));// Draw the block with the value of the corresponding cell
                    position++;                                                                                                 // Increase the position
                }
            }
        }
        /**
         * Returns the player's lives
         * @return The player's lives.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public int playersLives()
        {
            return this.player.GetLives();  // Return the player's lives
        }
        /**
         * Updates the level.
         * @param gameTime The time that passed since last update.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Update(GameTime gameTime)
        {
            this.UpdateBoards();                // Update the boards
            this.UpdateEnemies(gameTime);       // Update the enemies
            this.UpdatePlayer(gameTime);        // Update the player
            this.CheckCollisions();             // Calculate the collisions
        }
        /**
         * Updates the player
         * @param gameTime The time that passed since last update.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void UpdatePlayer(GameTime gameTime)
        {
            this.player.Update(gameTime);                                               // Update the player
            if (this.player.NeedsRespawn())                                             // Check if the player needs to respawn
            {
                this.player.Respawn();                                                  // Respawn the player
            }
            if (this.player.Landed() && this.player.IsMoving())                         // Check if the player landed and is moving
            {
                if (this.clearBoard.IsInsideBoard(this.player.GetPosition()))           // Check if the player is inside the board
                {
                    if (this.level % 2 == 1)                                            // Switch between logics, the block will turn off in even levels
                    {
                        if (!this.clearBoard.GetValue(this.player.GetPosition()))       // Check if the block is off
                        {
                            this.clearBoard.SetValue(this.player.GetPosition(), true);  // Turn it on
                            this.currentScore += 20 * (int)(Math.Floor((double)(this.level / 10) + 1));    // Add score
                        }
                    }
                    else                                                                // When the board can turn off
                    {
                        this.clearBoard.SetValue(this.player.GetPosition(), !this.clearBoard.GetValue(this.player.GetPosition()));      // Switch the block state
                        this.currentScore += 20 * (int)(Math.Floor((double)(this.level / 10) + 1));                                     // Add score
                    }
                    this.player.Land();                                                 // Land the player
                }
                else
                {
                    this.player.DropOff();                                              // Make the player fall if it is outside the board
                    this.player.LoseLives();                                            // Make the player lose a life
                }
            }
        }
        /**
         * Updates the boards.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void UpdateBoards()
        {
            this.enemyBoard.Clear();                                                            // Clear the enemy board
            for (int enemyCounter = 0; enemyCounter < this.enemies.Length; enemyCounter++)      // Loop through the enemies
            {
                if (this.enemies[enemyCounter].IsStanding())                                    // Check if the enemy is standing
                {
                    this.enemyBoard.SetValue(this.enemies[enemyCounter].GetPosition(), true);   // Set the board true in the enemy's position
                }
            }
        }
        /**
         * Updates the enemies.
         * @param timeSlice The time that passed since last update.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void UpdateEnemies(GameTime gameTime)
        {
            for (int enemyCounter = 0; enemyCounter < this.enemies.Length; enemyCounter++)              // Loop through the enemies
            {
                this.enemies[enemyCounter].Update(gameTime);                                            // Update the enemy
                if (this.enemies[enemyCounter].NeedsRespawn())                                          // Check if the enemy needs to respawn
                {
                    this.enemies[enemyCounter].Respawn();                                               // Respawn the enemy
                    this.enemies[enemyCounter].SetPosition(this.GetRandomMapPosition());                // Set the enemy position to a random position in the map
                }
                if (this.enemies[enemyCounter].IsReadyToMove())                                         // Check if the enemy is ready to move
                {
                    this.MoveEnemy(this.enemies[enemyCounter]);                                         // Move the enemy
                }
                if (this.enemies[enemyCounter].Landed() && this.enemies[enemyCounter].IsMoving())       // Check if the enemy landed and is moving
                {
                    if (this.enemyBoard.IsInsideBoard(this.enemies[enemyCounter].GetPosition()))        // Check if the position is inside the board
                    {
                        this.enemies[enemyCounter].Land();                                              // Land the enemy
                    }
                    else
                    {
                        this.enemies[enemyCounter].DropOff();                                           // Make the enemu fall
                    }
                }
            }
        }
        /**
         * Checks the collisions.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void CheckCollisions()
        {
            if (!this.player.IsMoving() && this.enemyBoard.GetValue(this.player.GetPosition()))                              // Check the player is not moving and the board is occupied at the players position
            {
                this.player.Hit();              // Hit the player
                this.player.LoseLives();        // Make the player lose lives
                for (int enemyCounter = 0; enemyCounter < this.enemies.Length; enemyCounter++)                                   // Loop through the enemies
                {
                    if (this.enemies[enemyCounter].GetPosition().X == 0 && this.enemies[enemyCounter].GetPosition().Y == 0)      // Check if the enemy is at the player's position
                    {
                        this.enemies[enemyCounter].Hit();                                                                        // Hit the enemy
                    }
                }
            }
            
        }
        /**
         * Draws the player.
         * @param spriteBatch The sprite batch that renders the player's sprites.
         * @param before A variable that tells if the method was called before the block rendering.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void DrawPlayer(SpriteBatch spriteBatch, bool before)
        {
            if (before && this.player.IsFalling())                                      // If drawn before and the player is falling
            {
                this.player.Draw(spriteBatch, this.GetGraphicPosition(this.player));    // Draw the player
            }
            else if (!before && !this.player.IsFalling())                               // If after and the player is not falling
            {
                this.player.Draw(spriteBatch, this.GetGraphicPosition(this.player));    // Draw the player
            }
        }
        /**
         * Draws the enemies.
         * @param spriteBatch The sprite batch that renders the enemies' sprites.
         * @param before A variable that tells if the method was called before the block rendering.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void DrawEnemies(SpriteBatch spriteBatch, bool before)
        {
            for (int enemyCounter = 0; enemyCounter < this.enemies.Length; enemyCounter++)                                  // Loop though the enemies
            {
                if (this.enemies[enemyCounter].CanBeDrawn())                                                                // Check if the enemy can be drawn
                {
                    if (before && this.enemies[enemyCounter].IsFalling())                                                   // Check if drawn before and the enemy is falling
                    {
                        this.enemies[enemyCounter].Draw(spriteBatch, this.GetGraphicPosition(this.enemies[enemyCounter]));  // Draw the enemy
                    }
                    else if (!before && !this.enemies[enemyCounter].IsFalling())                                            // Check if drawn after and is not falling
                    {
                        this.enemies[enemyCounter].Draw(spriteBatch, this.GetGraphicPosition(this.enemies[enemyCounter]));  // Draw the enemy
                    }
                }
            }
        }
        /**
         * Converts a board position into graphical position.
         * @param being The being to draw.
         * @return The position in graphical coordinates.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public Vector2 GetGraphicPosition(QbertBeing being)
        {
            Vector2 location = new Vector2(QbertLevel.initialXG + (85 * being.GetPosition().Y) - (85 * being.GetPosition().X) + (85 * being.GetJumpMovement().Y) - (85 * being.GetJumpMovement().X),   // Calculate the new vector with the graphical coordinates
                    QbertLevel.initialYG + (110 * being.GetPosition().Y) + (110 * being.GetPosition().X) + (110 * being.GetJumpMovement().Y) + (110 * being.GetJumpMovement().X));
            return location;        // Return the calculated Vector2
        }
        /**
         * Gets a random map position.
         * @return A Vector2 with a random valid position in the map.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        private Vector2 GetRandomMapPosition()
        {
            int x = this.random.Next(this.enemyBoard.GetWidth());           // Get a random x position
            int y = this.random.Next(this.clearBoard.GetWidth() - x);       // Get a random y position
            return new Vector2(x, y);                                       // Create the vector and return it.
        }
    }
}
