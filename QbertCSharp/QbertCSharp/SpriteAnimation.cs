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
     * Clas that Animates a sprite list.
     * 
     * @author Dario Urdapilleta
     * @version 1.0
     * @since 2022-11-09
     */
    internal class SpriteAnimation
    {
        private Texture2D texture;                  // The texture
        public Vector2 position = Vector2.Zero;     // The position
        public Color color = Color.White;           // The color
        public Vector2 origin;                      // The transformation origin
        public float rotation = 0f;                 // The rotation
        public float scale = 1f;                    // The scale
        public SpriteEffects spriteEffect;          // The sprite effects
        private Rectangle[] rectangles;             // The Rectangles
        private int frameIndex = 0;                 // The current frame
        private float timeElapsed;                  // The tie elapsed since last update
        public bool isLooping = false;              // Value to know if the animation loops
        private float timeToUpdate = 1f / 20f;      // Time to update
        /**
         * Creates a new instance of a SpriteAnimation given the texture and number of frames.
         * @param texture The animation texture
         * @param frames The number of frames
         * @param spriteWidth The sprite width
         * @param spriteHeight The sprite height
         * @para, start The starting line
         * @return A new instance of the SpriteAnimation class.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public SpriteAnimation(Texture2D texture, int frames, int spriteWidth, int spriteHeight, int start)
        {
            this.texture = texture;                                                 // Set the texture
            this.rectangles = new Rectangle[frames];                                // Create the rectangles

            for (int framesCounter = 0; framesCounter < frames; framesCounter++)    // Loop through the rectangles
            {
                this.rectangles[framesCounter] = new Rectangle(framesCounter * spriteWidth, start * spriteHeight, spriteWidth, spriteHeight);   // Store the sprite into the rectangle
            }
        }
        /**
         * Sets the frames per second
         * @param Frames per second
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void SetFramesPersecond(float framesPerSecond)
        {
            this.timeToUpdate = 1 / framesPerSecond;                // Set the time to update
        }
        /**
         * Draws the current texture
         * @param spriteBatch The Sprite Batch that renders the texture.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, this.rectangles[this.frameIndex], this.color, this.rotation, this.origin, this.scale, this.spriteEffect, 0f); // Display the frame
        }
        /**
         * Updates the sprite animation
         * @param gameTime The game time transcurred since last update.
         * 
         * @author Dario Urdapilleta
         * @version 1.0
         * @since 2022-11-09
         *	 
         */
        public void Update(GameTime gameTime)
        {
            this.timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;   // Add to the elapsed time the time passed since last update
            if (this.timeElapsed > this.timeToUpdate)                           // Check if it is time to update
            {
                this.timeElapsed -= this.timeToUpdate;                          // Reduce the time to update

                if (this.frameIndex < this.rectangles.Length - 1)               // Check if there's still more frames to animate
                    this.frameIndex++;                                          // Increase the frame

                else if (this.isLooping)                                        // Check if it's the last one and it should loop
                    this.frameIndex = 0;                                        // Loop the animation
            }
        }
    }
}
