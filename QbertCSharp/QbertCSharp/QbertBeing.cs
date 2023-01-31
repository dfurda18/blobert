using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QbertCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace QbertCSharp
{
    /**
     * This class represents A Qbert being such as the player or enemies.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     *
     */
    internal class QbertBeing
    {
        public const float maxHeight = 100f;             // The maximum jumping height
        public const float deltaHeight = 4f;             // The height change rate
        public const float deltaMovement = 0.02f;        // The movement animation frame rate
        public const float deltaJump = 0.1f;             // The jump animation frame rate

        public enum State
        {                               // Enum with all the animation states
            IDDLE,
            JUMPING_FRONT_UP_RIGHT,
            JUMPING_FRONT_UP_LEFT,
            JUMPING_FRONT_DOWN_RIGHT,
            JUMPING_FRONT_DOWN_LEFT,
            JUMPING_BACK_UP_LEFT,
            JUMPING_BACK_UP_RIGHT,
            JUMPING_BACK_DOWN_LEFT,
            JUMPING_BACK_DOWN_RIGHT
        }

        private Vector2 position;                               // The being's position
        private Texture2D texture;                              // The being's sprite list
        private Vector2 movementSlice;                          // The direction it is moving
        private Vector2 jumpMovement;                           // The actual placement of the being

        SpriteAnimation[] animations;                           // The animations

        private State state;                                    // The being's current state
        private float stateTime;                                // Time for the key frames
        private float height;                                   // The jump height
        private bool isMoving;                                  // True if the being is moving
        private bool landed;                                    // True if it has landed
        private bool isFalling;                                 // True if it's outside of the board
        protected bool pleaseRespawn;                           // True if it needs to respawn
        /**
         * Creates a new instance of a QbertBeing given the spritesheet information.
         * @param fileName The Spritesheet's file name.
         * @param start The start spritesheet position.
         * @param animationLength The length of each animation.
         * @return A new instance of the QbertBeing
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public QbertBeing(Texture2D sprites, int start, int animationLength)
        {
            this.state = QbertBeing.State.IDDLE;                                                // Start the state with IDDLE
            this.isMoving = false;                                                              // Make the being not move
            this.landed = true;                                                                 // It starts on the ground
            this.isFalling = false;                                                             // It starts in the board
            this.pleaseRespawn = false;                                                         // It doesn't need to be respawned
            this.movementSlice = new Vector2(0, 0);                                             // It's not moving
            this.jumpMovement = new Vector2(0, 0);                                              // It's not moving
            this.texture = sprites;                             					            // Load the sprites
            this.animations = new SpriteAnimation[5];                                           // Create the animations array
            for (int animationCounter = 0; animationCounter < this.animations.Length;animationCounter++)                    // Loop through the animations
            {
                this.animations[animationCounter] = new SpriteAnimation(sprites, 8, 60, 60, start + animationCounter);      // Create the animation
                this.animations[animationCounter].isLooping = true;                                                         // Set it to loop
                this.animations[animationCounter].SetFramesPersecond(8);                                                    // Set the frames per second
            }
            this.stateTime = 0f;                                                                // Reset the state time
            this.height = 0f;																	// Set the height as 0
	    }
	    /**
	        * Deletes all the graphical elements of a being
	        * 
	        * @author Dario Urdapilleta
	        * @version 1.0
	        * @since 2022-11-09
	        *	 
	        */
	    public void Dispose()
        {
            this.texture.Dispose();      // Delete the sprite list
        }
        /**
         * Returns true if the being needs to respawn.
         * @return True if it needs to be respwned.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool NeedsRespawn()
        {
            return this.pleaseRespawn;      // Please respawn this being
        }
        /**
         * Returns true if the being is standing.
         * @return True if the being is standing.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool IsStanding()
        {
            return this.state == QbertBeing.State.IDDLE;   // Make sure the position is not null, and the state is IDDLE
        }
        /**
         * Returns true if the being is moving.
         * @return True if the being is moving.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool IsMoving()
        {
            return this.isMoving;       // Return if the being is moving
        }
        /**
         * Returns true if the being has landed.
         * @return True if the being has landed.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool Landed()
        {
            return this.landed;         // Return true if the bieng has landed
        }
        /**
         * Returns true if the being is falling.
         * @return True if the being is falling.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public bool IsFalling()
        {
            return this.isFalling;      // Return true if the being is falling
        }
        /**
         * Sets the being's height
         * @param height The being's height.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void SetHeight(float height)
        {
            this.height = height;       // Set the height
        }
        /**
         * Returns the being's actual movement
         * @return The being's actual movement
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public Vector2 GetJumpMovement()
        {
            return this.jumpMovement;       // return the actual movement
        }
        /**
         * Returns the being's position as a Vector2
         * @return The being's position as a Vector2
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public Vector2 GetPosition()
        {
            return this.position;       // Return the position
        }
        /**
         * Sets the being's position
         * @param position The being's new position.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void SetPosition(Vector2 position)
        {
            this.position = position;   // Set the position
        }
        /**
         * Moves the being torwards a direction.
         * @param direction The direction the being is moving.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Move(int direction)
        {
            this.isMoving = true;                                       // Set isMoving to true
            this.landed = false;                                        // Set landed to false
            switch (direction)                                          // Do differently depending on the direction
            {
                case 0:                                                     // north west
                    this.movementSlice.Y = -QbertBeing.deltaMovement;       // Move negative on y
                    this.movementSlice.X = 0f;                              // Leave x unchanged
                    this.state = QbertBeing.State.JUMPING_BACK_UP_LEFT;     // Change state to JUMPING_BACK_UP_LEFT
                    break;
                case 1:                                                     // north east
                    this.movementSlice.X = -QbertBeing.deltaMovement;       // Move negative on x
                    this.movementSlice.Y = 0f;                              // Leave y unchanged
                    this.state = QbertBeing.State.JUMPING_BACK_UP_RIGHT;    // Change state to JUMPING_BACK_UP_RIGHT
                    break;
                case 2:                                                     // south east
                    this.movementSlice.Y = QbertBeing.deltaMovement;        // Move positive on y
                    this.movementSlice.X = 0f;                              // Leave x unchanged
                    this.state = QbertBeing.State.JUMPING_FRONT_UP_RIGHT;   // Change state to JUMPING_FRONT_UP_RIGHT
                    break;
                default:                                                    // south west
                    this.movementSlice.X = QbertBeing.deltaMovement;        // Move positive on x
                    this.movementSlice.Y = 0f;                              // Leave y unchanged
                    this.state = QbertBeing.State.JUMPING_FRONT_UP_LEFT;    // Change state to JUMPING_FRONT_UP_LEFT
                    break;
            }
            this.stateTime = 0f;                                        // Reset the state time
        }
        /**
         * Updates the being
         * @param gameTime The game time transcurred since last update.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         */
        public void Update (GameTime gameTime)
        {
            this.stateTime += (float)gameTime.ElapsedGameTime.TotalSeconds;                     // Increase the animation state
            switch (this.state)                                                                 // Do different depending on the state
            {
                case State.IDDLE:                                                               // When it is IDDLE
                    this.height = 0f;                                                           // Set the height to 0
                    break;
                case State.JUMPING_FRONT_UP_RIGHT:                                              // When it is JUMPING_FRONT_UP_RIGHT
                    this.height += QbertBeing.deltaHeight;                                      // Increase the height
                    break;
                case State.JUMPING_FRONT_UP_LEFT:                                               // When it is JUMPING_FRONT_UP_LEFT
                    this.height += QbertBeing.deltaHeight;                                      // Increase the height
                    break;
                case State.JUMPING_FRONT_DOWN_RIGHT:                                            // When it is JUMPING_FRONT_DOWN_RIGHT
                    this.height -= QbertBeing.deltaHeight;                                      // Decrease the height
                    break;
                case State.JUMPING_FRONT_DOWN_LEFT:                                             // When it is JUMPING_FRONT_DOWN_LEFT
                    this.height -= QbertBeing.deltaHeight;                                      // Decrease the height
                    break;
                case State.JUMPING_BACK_UP_LEFT:                                                // When it is JUMPING_BACK_UP_LEFT
                    this.height += QbertBeing.deltaHeight;                                      // Increase the height
                    break;
                case State.JUMPING_BACK_UP_RIGHT:                                               // When it is JUMPING_BACK_UP_RIGHT
                    this.height += QbertBeing.deltaHeight;                                      // Increase the height
                    break;
                case State.JUMPING_BACK_DOWN_LEFT:                                              // When it is IDDLE
                    this.height -= QbertBeing.deltaHeight;                                      // Decrease the height
                    break;
                case State.JUMPING_BACK_DOWN_RIGHT:                                             // When it is IDDLE
                    this.height -= QbertBeing.deltaHeight;                                      // Decrease the height
                    break;
                default:
                    this.height = 0f;                                                           // Set the height to 0
                    break;
            }
            if (this.height >= QbertBeing.maxHeight)                                            // If the being has reached the max height
            {
                switch (this.state)                                                             // Do differently depending on the state
                {
                    case State.JUMPING_FRONT_UP_RIGHT:                                          // JUMPING_FRONT_UP_RIGHT
                        this.state = QbertBeing.State.JUMPING_FRONT_DOWN_RIGHT;                 // Change to JUMPING_FRONT_DOWN_RIGHT
                        break;
                    case State.JUMPING_FRONT_UP_LEFT:                                           // JUMPING_FRONT_UP_LEFT
                        this.state = QbertBeing.State.JUMPING_FRONT_DOWN_LEFT;                  // Change to JUMPING_FRONT_DOWN_LEFT
                        break;
                    case State.JUMPING_BACK_UP_LEFT:                                            // JUMPING_BACK_UP_LEFT
                        this.state = QbertBeing.State.JUMPING_BACK_DOWN_LEFT;                   // Change to JUMPING_BACK_DOWN_LEFT
                        break;
                    case State.JUMPING_BACK_UP_RIGHT:                                           // JUMPING_BACK_UP_RIGHT
                        this.state = QbertBeing.State.JUMPING_BACK_DOWN_RIGHT;                  // Change to JUMPING_BACK_DOWN_RIGHT
                        break;
                    default:                                                                    // Otherwise, do nothing
                        break;
                }
            }
            this.jumpMovement.X += this.movementSlice.X;                                        // Move the being on the x axis
            this.jumpMovement.Y += this.movementSlice.Y;                                        // Move the being to the y axis
            if (!this.isFalling && this.height < 0)                                             // Check if it's not falling and it has landed
            {
                if (this.jumpMovement.X >= 1f)                                                  // Check the movement on x is more than 1
                {
                    this.position.X++;                                                          // Increase the position in x
                }
                if (this.jumpMovement.X <= -1f)                                                 // Check the movement on x is lees than -1
                {
                    this.position.X--;                                                          // Decrease X
                }
                if (this.jumpMovement.Y >= 1f)                                                  // Check the y movement is greater than 1
                {
                    this.position.Y++;                                                          // Increase y
                }
                if (this.jumpMovement.Y <= -1f)                                                 // Check the y movement is less than -1
                {
                    this.position.Y--;                                                          // Decrease y
                }
                this.landed = true;                                                             // Set landed to true
            }
            if (this.height <= -300f)                                                           // Check if the height has gone less than -300
            {
                this.pleaseRespawn = true;                                                      // Request to respawn
            }
            for (int animationCounter = 0; animationCounter < this.animations.Length; animationCounter++)                    // Loop through the animations
            {
                this.animations[animationCounter].Update(gameTime);                             // Update the animation
            }
        }
        /**
         * Draws the being
         * @param spriteBatch The sprite batch that draws it.
         * @param graphicPosition The being's acutal position
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Draw(SpriteBatch spriteBatch, Vector2 graphicPosition)
        {
            for (int animationCounter = 0; animationCounter < this.animations.Length; animationCounter++)    // Loop through the animations
            {
                this.animations[animationCounter].position = graphicPosition;                   // Update the animations position
                this.animations[animationCounter].position.Y -= this.height;                    // Add the height to the position

            }
            switch (this.state)                                                                 // Do different depending on the state
            {
                case State.IDDLE:                                                               // When it is IDDLE
                    this.animations[0].Draw(spriteBatch);                                       // Show the idle animation
                    break;
                case State.JUMPING_FRONT_UP_RIGHT:                                              // When it is JUMPING_FRONT_UP_RIGHT
                case State.JUMPING_FRONT_DOWN_RIGHT:                                            // When it is JUMPING_FRONT_DOWN_RIGHT                                                         
                    this.animations[1].Draw(spriteBatch);                                       // Show the jump front right anumation
                    break;
                case State.JUMPING_FRONT_UP_LEFT:                                               // When it is JUMPING_FRONT_UP_LEFT
                case State.JUMPING_FRONT_DOWN_LEFT:                                             // When it is JUMPING_FRONT_DOWN_LEFT
                    this.animations[2].Draw(spriteBatch);                                       // Show the jump front left anumation
                    break;
                case State.JUMPING_BACK_UP_LEFT:                                                // When it is JUMPING_BACK_UP_LEFT
                case State.JUMPING_BACK_DOWN_LEFT:                                              // When it is JUMPING_BACK_DOWN_LEFT
                    this.animations[4].Draw(spriteBatch);                                       // Show the jump front left anumation
                    break;
                case State.JUMPING_BACK_UP_RIGHT:                                               // When it is JUMPING_BACK_UP_RIGHT
                case State.JUMPING_BACK_DOWN_RIGHT:                                             // When it is JUMPING_BACK_DOWN_RIGHT
                    this.animations[3].Draw(spriteBatch);                                       // Show the jump front left anumation
                    break;
                default:
                    this.animations[0].Draw(spriteBatch);                                       // Show the idle anumation
                    break;
            }
        }
        /**
         * Hits the being.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Hit()
        {
            this.pleaseRespawn = true;      // Request respawn
            this.isMoving = true;           // Set moving to true
            this.ResetValues();             // Reset the values
        }
        /**
         * Resets the values for a new respawn.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void ResetValues()
        {
            this.jumpMovement.X = 0f;       // Movement x to 0
            this.movementSlice.X = 0f;      // Movement slice x to 0
            this.jumpMovement.Y = 0f;       // Movement y to 0
            this.movementSlice.Y = 0f;      // Movement slice y to 0
        }
        /**
         * Sets the being to fall definitely.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void DropOff()
        {
            this.ResetValues();             // Reset the values
            this.isFalling = true;          // Set falling to true
            this.landed = false;            // Set landed to false
        }
        /**
         * Lands the being.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Land()
        {
            if (this.height <= 0)                       // Check if the height is less than 0
            {
                this.height = 0f;                       // Set the height to 0
                this.state = QbertBeing.State.IDDLE;    // Set the state to IDDLE
            }
            this.ResetValues();                         // Reset the values
            this.isMoving = false;                      // Set isMoving to false
        }
        /**
         * Respawns the being.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Respawn()
        {
            this.SetHeight(QbertBeing.maxHeight);                       // Set the height to the max
            this.state = QbertBeing.State.JUMPING_FRONT_DOWN_RIGHT;     // Change the state to	 JUMPING_FRONT_DOWN_RIGHT
            this.landed = false;                                        // Set landed to false
            this.isMoving = true;                                       // Set isMoving to true
            this.isFalling = false;                                     // Set is Falling to false
            this.pleaseRespawn = false;                                 // Request to respawn
            this.stateTime = 0.5f;                                      // Set the state time to 0.5
        }
    }
}
