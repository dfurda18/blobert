package com.urdapilleta.qbert;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Animation;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.Vector2;
/**
 * This class represents A Qbert being such as the player or enemies.
 * 
 * @author Dario Urdapilleta
 * @version 1.0
 * @since 2022-11-09
 *
 */
public class QbertBeing {
	private static final float maxHeight = 100f;				// The maximum jumping height
	private static final float deltaHeight = 4f;				// The height change rate
	private static final float deltaMovement = 0.02f;		// The movement animation frame rate
	private static final float deltaJump = 0.1f;				// The jump animation frame rate
	
	public static enum State {								// Enum with all the animation states
		IDDLE, 
		JUMPING_FRONT_UP_RIGHT, 
		JUMPING_FRONT_UP_LEFT, 
		JUMPING_FRONT_DOWN_RIGHT, 
		JUMPING_FRONT_DOWN_LEFT, 
		JUMPING_BACK_UP_LEFT, 
		JUMPING_BACK_UP_RIGHT, 
		JUMPING_BACK_DOWN_LEFT, 
		JUMPING_BACK_DOWN_RIGHT}
	
	private Vector2 position;								// The being's position
	private Texture spriteList;								// The being's sprite list
	private Vector2 movementSlice;							// The direction it is moving
	private Vector2 jumpMovement;							// The actual placement of the being
	
	Animation<TextureRegion> iddleAnimation;				// The idle animation
	Animation<TextureRegion> jumpFrontLeftAnimation;		// The jump front left animation
	Animation<TextureRegion> jumpFrontRightAnimation;		// The jump front right animation
	Animation<TextureRegion> jumpBackLeftAnimation;			// The jump back left animation
	Animation<TextureRegion> jumpBackRightAnimation;		// The jump back right animation
	
	private State state;									// The being's current state
	private float stateTime;								// Time for the key frames
	private float height;									// The jump height
	private boolean isMoving;								// True if the being is moving
	private boolean landed;									// True if it has landed
	private boolean isFalling;								// True if it's outside of the board
	protected boolean pleaseRespawn;						// True if it needs to respawn
	/**
	 * Creates a new instance of a QbertBeing given the spritesheet information.
	 * @param fileName The Spritesheet's file name.
	 * @param start The start spritesheet position.
	 * @param animationLength The length of each animation.
	 * @param verticalSpritesPerSheet The number of vertical sptrites in total in the sheet.
	 * @return A new instance of the QbertBeing
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public QbertBeing (String fileName, int start, int animationLength, int verticalSpritesPerSheet) {
		this.state = QbertBeing.State.IDDLE;												// Start the state with IDDLE
		this.isMoving = false;																// Make the being not move
		this.landed = true;																	// It starts on the ground
		this.isFalling = false;																// It starts in the board
		this.pleaseRespawn = false;															// It doesn't need to be respawned
		this.movementSlice = new Vector2(0,0);												// It's not moving
		this.jumpMovement = new Vector2(0,0);												// It's not moving
		this.spriteList = new Texture(Gdx.files.internal("AnimationSpritelist.png"));		// Load the sprites
		TextureRegion[][] tmp = TextureRegion.split(spriteList,								// Create a temporal sprite region each line is an animation
				this.spriteList.getWidth() / animationLength,
				this.spriteList.getHeight() / verticalSpritesPerSheet);
		TextureRegion[] iddleFrames = new TextureRegion[animationLength];					// Create the iddle texture region
		TextureRegion[] jumpFrontLeftFrames = new TextureRegion[animationLength];			// Create the jump front left texture region
		TextureRegion[] jumpFrontRightFrames = new TextureRegion[animationLength];			// Create the jump front right texture region
		TextureRegion[] jumpBackLeftFrames = new TextureRegion[animationLength];			// Create the jump back left texture region
		TextureRegion[] jumpBackRightFrames = new TextureRegion[animationLength];			// Create the jump back right texture region
		int index = 0;																		// Reset the index
		int frameCounterX, frameCounterY;													// Declare the counters used to loop
		for (frameCounterX = 0; frameCounterX < animationLength; frameCounterX++) {			// Loop for each animation frame
			iddleFrames[index++] = tmp[start][frameCounterX];								// Set the frame to the correct array
		}
		this.iddleAnimation = new Animation<TextureRegion>(0.088f, iddleFrames);			// Create the animation object
		index = 0;																			// Reset the index
		for (frameCounterX = 0; frameCounterX < animationLength; frameCounterX++) {			// Loop for each animation frame
			jumpFrontRightFrames[index++] = tmp[start + 1][frameCounterX];					// Set the frame to the correct array
		}
		this.jumpFrontRightAnimation = new Animation<TextureRegion>(QbertBeing.deltaJump, jumpFrontRightFrames);	// Create the animation object
		index = 0;																			// Reset the index|
		for (frameCounterX = 0; frameCounterX < animationLength; frameCounterX++) {			// Loop for each animation frame
			jumpFrontLeftFrames[index++] = tmp[start + 2][frameCounterX];					// Set the frame to the correct array
		}
		this.jumpFrontLeftAnimation = new Animation<TextureRegion>(QbertBeing.deltaJump, jumpFrontLeftFrames);		// Create the animation object
		index = 0;																			// Reset the index
		for (frameCounterX = 0; frameCounterX < animationLength; frameCounterX++) {			// Loop for each animation frame
			jumpBackLeftFrames[index++] = tmp[start + 3][frameCounterX];					// Set the frame to the correct array
		}
		this.jumpBackLeftAnimation = new Animation<TextureRegion>(QbertBeing.deltaJump, jumpBackLeftFrames);		// Create the animation object
		index = 0;																			// Reset the index
		for (frameCounterX = 0; frameCounterX < animationLength; frameCounterX++) {			// Loop for each animation frame
			jumpBackRightFrames[index++] = tmp[start + 4][frameCounterX];					// Set the frame to the correct array
		}
		this.jumpBackRightAnimation = new Animation<TextureRegion>(QbertBeing.deltaJump, jumpBackRightFrames);		// Create the animation object
		this.stateTime = 0f;																// Reset the state time
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
	public void dispose() {
		this.spriteList.dispose();		// Delete the sprite list
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
	public boolean needsRespawn() {
		return this.pleaseRespawn;		// Please respawn this being
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
	public boolean isStanding() {
		return this.position != null && this.state == QbertBeing.State.IDDLE;	// Make sure the position is not null, and the state is IDDLE
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
	public boolean isMoving() {
		return this.isMoving;		// Return if the being is moving
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
	public boolean landed() {
		return this.landed;			// Return true if the bieng has landed
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
	public boolean isFalling() {
		return this.isFalling;		// Return true if the being is falling
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
	public void setHeight(float height) {
		this.height = height;		// Set the height
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
	public Vector2 getJumpMovement() {
		return this.jumpMovement;		// return the actual movement
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
	public Vector2 getPosition() {
		return this.position;		// Return the position
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
	public void setPosition(Vector2 position) {
		this.position = position;	// Set the position
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
	public void move(int direction) {
		this.isMoving = true;										// Set isMoving to true
		this.landed = false;										// Set landed to false
		switch(direction)											// Do differently depending on the direction
		{
		case 0: 													// north west
			this.movementSlice.y = -QbertBeing.deltaMovement;		// Move negative on y
			this.movementSlice.x = 0f;								// Leave x unchanged
			this.state = QbertBeing.State.JUMPING_BACK_UP_LEFT;		// Change state to JUMPING_BACK_UP_LEFT
			break;
		case 1: 													// north east
			this.movementSlice.x = -QbertBeing.deltaMovement;		// Move negative on x
			this.movementSlice.y = 0f;								// Leave y unchanged
			this.state = QbertBeing.State.JUMPING_BACK_UP_RIGHT;	// Change state to JUMPING_BACK_UP_RIGHT
			break;
		case 2: 													// south east
			this.movementSlice.y = QbertBeing.deltaMovement;		// Move positive on y
			this.movementSlice.x = 0f;								// Leave x unchanged
			this.state = QbertBeing.State.JUMPING_FRONT_UP_RIGHT;	// Change state to JUMPING_FRONT_UP_RIGHT
			break;
		default: 													// south west
			this.movementSlice.x = QbertBeing.deltaMovement;		// Move positive on x
			this.movementSlice.y = 0f;								// Leave y unchanged
			this.state = QbertBeing.State.JUMPING_FRONT_UP_LEFT;	// Change state to JUMPING_FRONT_UP_LEFT
			break;
		}
		this.stateTime = 0f;										// Reset the state time
	}
	/**
	 * Updates and draws the being
	 * @param spriteBatch The sprite batch that draws it.
	 * @param graphicPosition The being's acutal position
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void draw(SpriteBatch spriteBatch, Vector2 graphicPosition) {
		if(this.position != null) {															// Ensure the current position is not null
			this.stateTime += Gdx.graphics.getDeltaTime();									// Increase the animation state
			TextureRegion currentFrame;														// Declare a Texture region for the current grame
			switch(this.state)																// Do different depending on the state
			{
			case IDDLE:																		// When it is IDDLE
				currentFrame = iddleAnimation.getKeyFrame(this.stateTime, true);			// Set the corresponding frame
				this.height = 0f;															// Set the height to 0
				break;
			case JUMPING_FRONT_UP_RIGHT:													// When it is JUMPING_FRONT_UP_RIGHT
				currentFrame = jumpFrontRightAnimation.getKeyFrame(this.stateTime, true);	// Set the corresponding frame
				this.height += QbertBeing.deltaHeight;										// Increase the height
				break;
			case JUMPING_FRONT_UP_LEFT:														// When it is JUMPING_FRONT_UP_LEFT
				currentFrame = jumpFrontLeftAnimation.getKeyFrame(this.stateTime, true);	// Set the corresponding frame
				this.height += QbertBeing.deltaHeight;										// Increase the height
				break;
			case JUMPING_FRONT_DOWN_RIGHT:													// When it is JUMPING_FRONT_DOWN_RIGHT
				currentFrame = jumpFrontRightAnimation.getKeyFrame(this.stateTime, true);	// Set the corresponding frame
				this.height -= QbertBeing.deltaHeight;										// Decrease the height
				break;
			case JUMPING_FRONT_DOWN_LEFT: 													// When it is JUMPING_FRONT_DOWN_LEFT
				currentFrame = jumpFrontLeftAnimation.getKeyFrame(this.stateTime, true);	// Set the corresponding frame
				this.height -= QbertBeing.deltaHeight;										// Decrease the height
				break;
			case JUMPING_BACK_UP_LEFT:														// When it is JUMPING_BACK_UP_LEFT
				currentFrame = jumpBackLeftAnimation.getKeyFrame(this.stateTime, true);		// Set the corresponding frame
				this.height += QbertBeing.deltaHeight;										// Increase the height
				break;
			case JUMPING_BACK_UP_RIGHT:														// When it is JUMPING_BACK_UP_RIGHT
				currentFrame = jumpBackRightAnimation.getKeyFrame(this.stateTime, true);	// Set the corresponding frame
				this.height += QbertBeing.deltaHeight;										// Increase the height
				break;
			case JUMPING_BACK_DOWN_LEFT:													// When it is IDDLE
				currentFrame = jumpBackLeftAnimation.getKeyFrame(this.stateTime, true);		// Set the corresponding frame
				this.height -= QbertBeing.deltaHeight;										// Decrease the height
				break;
			case JUMPING_BACK_DOWN_RIGHT:													// When it is IDDLE
				currentFrame = jumpBackRightAnimation.getKeyFrame(this.stateTime, true);	// Set the corresponding frame
				this.height -= QbertBeing.deltaHeight;										// Decrease the height
				break;
			default:
				currentFrame = iddleAnimation.getKeyFrame(this.stateTime, true);			// Default would be IDDLE
				this.height = 0f;															// Set the height to 0
				break;
			}
			if(this.height >= QbertBeing.maxHeight) {										// If the being has reached the max height
				switch(this.state)															// Do differently depending on the state
				{
				case JUMPING_FRONT_UP_RIGHT:												// JUMPING_FRONT_UP_RIGHT
					this.state = QbertBeing.State.JUMPING_FRONT_DOWN_RIGHT;					// Change to JUMPING_FRONT_DOWN_RIGHT
					break;
				case JUMPING_FRONT_UP_LEFT:													// JUMPING_FRONT_UP_LEFT
					this.state = QbertBeing.State.JUMPING_FRONT_DOWN_LEFT;					// Change to JUMPING_FRONT_DOWN_LEFT
					break;
				case JUMPING_BACK_UP_LEFT:													// JUMPING_BACK_UP_LEFT
					this.state = QbertBeing.State.JUMPING_BACK_DOWN_LEFT;					// Change to JUMPING_BACK_DOWN_LEFT
					break;
				case JUMPING_BACK_UP_RIGHT:													// JUMPING_BACK_UP_RIGHT
					this.state = QbertBeing.State.JUMPING_BACK_DOWN_RIGHT;					// Change to JUMPING_BACK_DOWN_RIGHT
					break;
				default:																	// Otherwise, do nothing
					break;
				}
			}
			spriteBatch.draw(currentFrame, graphicPosition.x, graphicPosition.y + this.height);		// Draw the being
			this.jumpMovement.x += this.movementSlice.x;									// Move the being on the x axis
			this.jumpMovement.y += this.movementSlice.y;									// Move the being to the y axis
			if(!this.isFalling && this.height < 0) {										// Check if it's not falling and it has landed
				if(this.jumpMovement.x >= 1f) {												// Check the movement on x is more than 1
					this.position.x++;														// Increase the position in x
				}
				if(this.jumpMovement.x <= -1f) {											// Check the movement on x is lees than -1
					this.position.x--;														// Decrease X
				}
				if(this.jumpMovement.y >= 1f) {												// Check the y movement is greater than 1
					this.position.y++;														// Increase y
				}
				if(this.jumpMovement.y <= -1f) {											// Check the y movement is less than -1
					this.position.y--;														// Decrease y
				}
				this.landed = true;															// Set landed to true
			}
			if (this.height <= -300.f) {													// Check if the height has gone less than -300
				this.pleaseRespawn = true;													// Request to respawn
			}
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
	public void hit() {
		this.pleaseRespawn = true;		// Request respawn
		this.isMoving = true;			// Set moving to true
		this.resetValues();				// Reset the values
	}
	/**
	 * Resets the values for a new respawn.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void resetValues() {
		this.jumpMovement.x = 0f;		// Movement x to 0
		this.movementSlice.x = 0f;		// Movement slice x to 0
		this.jumpMovement.y = 0f;		// Movement y to 0
		this.movementSlice.y = 0f;		// Movement slice y to 0
	}
	/**
	 * Sets the being to fall definitely.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void dropOff() {
		this.resetValues();				// Reset the values
		this.isFalling = true;			// Set falling to true
		this.landed = false;			// Set landed to false
	}
	/**
	 * Lands the being.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void land() {
		if(this.height <= 0) {						// Check if the height is less than 0
			this.height = 0f;						// Set the height to 0
			this.state = QbertBeing.State.IDDLE;	// Set the state to IDDLE
		}
		this.resetValues();							// Reset the values
		this.isMoving = false;						// Set isMoving to false
	}
	/**
	 * Respawns the being.
	 * 
	 * @author Dario Urdapilleta
	 * @version 1.0
	 * @since 2022-11-09
	 *	 
	 */
	public void respawn() {
		this.setHeight(QbertBeing.maxHeight);						// Set the height to the max
		this.state = QbertBeing.State.JUMPING_FRONT_DOWN_RIGHT;		// Change the state to	 JUMPING_FRONT_DOWN_RIGHT
		this.landed = false;										// Set landed to false
		this.isMoving = true;										// Set isMoving to true
		this.isFalling = false;										// Set is Falling to false
		this.pleaseRespawn = false;									// Request to respawn
		this.stateTime = 0.5f;										// Set the state time to 0.5
	}
}